using System;
using System.IO;
using System.Collections.Generic;

namespace AIkailo.Data.DBCore
{
    /// <summary>
    /// Record storage service that store data in form of records, each
    /// record made up from one or several blocks
    /// </summary>
    public class RecordStorage : IRecordStorage
    {
        private readonly IBlockStorage _storage;
        private const int MAX_RECORD_SIZE = 4194304; // 4MB

        private const int HEADER_FIELD_NEXT_BLOCK_ID = 0;
        private const int HEADER_FIELD_RECORD_LENGTH = 1;
        private const int HEADER_FIELD_BLOCK_CONTENT_LENGTH = 2;
        private const int HEADER_FIELD_PREVIOUS_BLOCK_ID = 3;
        private const int HEADER_FIELD_IS_DELETED = 4;

        //
        // Constructors
        //

        public RecordStorage(IBlockStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));

            if (storage.BlockHeaderSize < 48)
            {
                throw new ArgumentException("Record storage needs at least 48 header bytes");
            }
        }

        //
        // Public Methods
        //

        public virtual byte[] Find(uint recordId)
        {
            // First grab the block
            using (var block = _storage.Find(recordId))
            {
                if (block == null)
                {
                    return null;
                }

                // If this is a deleted block then ignore it
                if (1L == block.GetHeader(HEADER_FIELD_IS_DELETED))
                {
                    return null;
                }

                // If this block is a child block then also ignore it
                if (0L != block.GetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID))
                {
                    return null;
                }

                // Grab total record size and allocate coressponded memory
                var totalRecordSize = block.GetHeader(HEADER_FIELD_RECORD_LENGTH);
                if (totalRecordSize > MAX_RECORD_SIZE)
                {
                    throw new NotSupportedException("Unexpected record length: " + totalRecordSize);
                }
                var data = new byte[totalRecordSize];
                var bytesRead = 0;

                // Now start filling data
                IBlock currentBlock = block;
                while (true)
                {
                    uint nextBlockId;

                    using (currentBlock)
                    {
                        var thisBlockContentLength = currentBlock.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH);
                        if (thisBlockContentLength > _storage.BlockContentSize)
                        {
                            throw new InvalidDataException("Unexpected block content length: " + thisBlockContentLength);
                        }

                        // Read all available content of current block
                        currentBlock.Read(dst: data, dstOffset: bytesRead, srcOffset: 0, count: (int)thisBlockContentLength);

                        // Update number of bytes read
                        bytesRead += (int)thisBlockContentLength;

                        // Move to the next block if there is any
                        nextBlockId = (uint)currentBlock.GetHeader(HEADER_FIELD_NEXT_BLOCK_ID);
                        if (nextBlockId == 0)
                        {
                            return data;
                        }
                    }// Using currentBlock

                    currentBlock = this._storage.Find(nextBlockId);
                    if (currentBlock == null)
                    {
                        throw new InvalidDataException("Block not found by id: " + nextBlockId);
                    }
                }
            }
        }

        public virtual uint Create(Func<uint, byte[]> dataGenerator)
        {
            if (dataGenerator == null) { throw new ArgumentNullException(nameof(dataGenerator)); }

            using (var firstBlock = AllocateBlock())
            {
                var returnId = firstBlock.Id;

                // Alright now begin writing data
                var data = dataGenerator(returnId);
                var dataWritten = 0;
                var dataTobeWritten = data.Length;
                firstBlock.SetHeader(HEADER_FIELD_RECORD_LENGTH, dataTobeWritten);

                // If no data tobe written,
                // return this block straight away
                if (dataTobeWritten == 0)
                {
                    return returnId;
                }

                // Otherwise continue to write data until completion
                IBlock currentBlock = firstBlock;
                while (dataWritten < dataTobeWritten)
                {
                    IBlock nextBlock = null;

                    using (currentBlock)
                    {
                        // Write as much as possible to this block
                        var thisWrite = Math.Min(_storage.BlockContentSize, dataTobeWritten - dataWritten);
                        currentBlock.Write(data, dataWritten, 0, thisWrite);
                        currentBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, thisWrite);
                        dataWritten += thisWrite;

                        // If still there are data tobe written,
                        // move to the next block
                        if (dataWritten < dataTobeWritten)
                        {
                            nextBlock = AllocateBlock();
                            var success = false;
                            try
                            {
                                nextBlock.SetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID, currentBlock.Id);
                                currentBlock.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, nextBlock.Id);
                                success = true;
                            }
                            finally
                            {
                                if ((false == success) && (nextBlock != null))
                                {
                                    nextBlock.Dispose();
                                    nextBlock = null;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    } // Using currentBlock

                    // Move to the next block if possible
                    if (nextBlock != null)
                    {
                        currentBlock = nextBlock;
                    }
                }

                // return id of the first block that got dequeued
                return returnId;
            }
        }

        public virtual uint Create(byte[] data)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            return Create(recordId => data);
        }

        public virtual uint Create()
        {
            using (var firstBlock = AllocateBlock())
            {
                return firstBlock.Id;
            }
        }

        public virtual void Delete(uint recordId)
        {
            using (var block = _storage.Find(recordId))
            {
                IBlock currentBlock = block;
                while (true)
                {
                    IBlock nextBlock = null;

                    using (currentBlock)
                    {
                        MarkAsFree(currentBlock.Id);
                        currentBlock.SetHeader(HEADER_FIELD_IS_DELETED, 1L);

                        var nextBlockId = (uint)currentBlock.GetHeader(HEADER_FIELD_NEXT_BLOCK_ID);
                        if (nextBlockId == 0)
                        {
                            break;
                        }
                        else
                        {
                            nextBlock = _storage.Find(nextBlockId);
                            if (currentBlock == null)
                            {
                                throw new InvalidDataException("Block not found by id: " + nextBlockId);
                            }
                        }
                    }// Using currentBlock

                    // Move to next block
                    if (nextBlock != null)
                    {
                        currentBlock = nextBlock;
                    }
                }
            }
        }

        public virtual void Update(uint recordId, byte[] data)
        {
            var written = 0;
            var total = data.Length;
            var blocks = FindBlocks(recordId);
            var blocksUsed = 0;
            var previousBlock = (IBlock)null;

            try
            {
                // Start writing block by block..
                while (written < total)
                {
                    // Bytes to be written in this block
                    var bytesToWrite = Math.Min(total - written, _storage.BlockContentSize);

                    // Get the block where the first byte of remaining data will be written to
                    var blockIndex = (int)Math.Floor((double)written / (double)_storage.BlockContentSize);

                    // Find the block to write to:
                    // If `blockIndex` exists in `blocks`, then write into it,
                    // otherwise allocate a new one for writting
                    var target = (IBlock)null;
                    if (blockIndex < blocks.Count)
                    {
                        target = blocks[blockIndex];
                    }
                    else
                    {
                        target = AllocateBlock();
                        if (target == null)
                        {
                            throw new Exception("Failed to allocate new block");
                        }
                        blocks.Add(target);
                    }

                    // Link with previous block
                    if (previousBlock != null)
                    {
                        previousBlock.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, target.Id);
                        target.SetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID, previousBlock.Id);
                    }

                    // Write data
                    target.Write(src: data, srcOffset: written, dstOffset: 0, count: bytesToWrite);
                    target.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, bytesToWrite);
                    target.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, 0);
                    if (written == 0)
                    {
                        target.SetHeader(HEADER_FIELD_RECORD_LENGTH, total);
                    }

                    // Get ready fr next loop
                    blocksUsed++;
                    written += bytesToWrite;
                    previousBlock = target;
                }

                // After writing, delete off any unused blocks
                if (blocksUsed < blocks.Count)
                {
                    for (var i = blocksUsed; i < blocks.Count; i++)
                    {
                        MarkAsFree(blocks[i].Id);
                    }
                }
            }
            finally
            {
                // Always dispose all fetched blocks after finish using them
                foreach (var block in blocks)
                {
                    block.Dispose();
                }
            }
        }

        //
        // Private Methods
        //

        /// <summary>
        /// Find all blocks of given record, return these blocks in order.
        /// </summary>
        /// <param name="recordId">Record identifier.</param>
        List<IBlock> FindBlocks(uint recordId)
        {
            var blocks = new List<IBlock>();
            var success = false;

            try
            {
                var currentBlockId = recordId;

                do
                {
                    // Grab next block
                    var block = _storage.Find(currentBlockId);
                    if (null == block)
                    {
                        // Special case: if block #0 never created, then attempt to create it
                        if (currentBlockId == 0)
                        {
                            block = _storage.CreateNew();
                        }
                        else
                        {
                            throw new Exception("Block not found by id: " + currentBlockId);
                        }
                    }
                    blocks.Add(block);

                    // If this is a deleted block then ignore the fuck out of it
                    if (1L == block.GetHeader(HEADER_FIELD_IS_DELETED))
                    {
                        throw new InvalidDataException("Block not found: " + currentBlockId);
                    }

                    // Move next
                    currentBlockId = (uint)block.GetHeader(HEADER_FIELD_NEXT_BLOCK_ID);
                } while (currentBlockId != 0);

                success = true;
                return blocks;
            }
            finally
            {
                // Incase shit happens, dispose all fetched blocks
                if (false == success)
                {
                    foreach (var block in blocks)
                    {
                        block.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Allocate new block for use, either by dequeueing an exising non-used block
        /// or creating a new one
        /// </summary>
        /// <returns>Newly allocated block ready to use.</returns>
        IBlock AllocateBlock()
        {
            uint resuableBlockId;
            IBlock newBlock;
            if (false == TryFindFreeBlock(out resuableBlockId))
            {
                newBlock = _storage.CreateNew();
                if (newBlock == null)
                {
                    throw new Exception("Failed to create new block");
                }
            }
            else
            {
                newBlock = _storage.Find(resuableBlockId);
                if (newBlock == null)
                {
                    throw new InvalidDataException("Block not found by id: " + resuableBlockId);
                }
                newBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, 0L);
                newBlock.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, 0L);
                newBlock.SetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID, 0L);
                newBlock.SetHeader(HEADER_FIELD_RECORD_LENGTH, 0L);
                newBlock.SetHeader(HEADER_FIELD_IS_DELETED, 0L);
            }
            return newBlock;
        }

        bool TryFindFreeBlock(out uint blockId)
        {
            blockId = 0;
            IBlock lastBlock, secondLastBlock;
            GetSpaceTrackingBlock(out lastBlock, out secondLastBlock);

            using (lastBlock)
            using (secondLastBlock)
            {
                // If this block is empty, then goto previous block
                var currentBlockContentLength = lastBlock.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH);
                if (currentBlockContentLength == 0)
                {
                    // If there is no previous block, return false to indicate we can't dequeu
                    if (secondLastBlock == null)
                    {
                        return false;
                    }

                    // Dequeue an uint from previous block, then mark current block as free
                    blockId = ReadUInt32FromTrailingContent(secondLastBlock);

                    // Back off 4 bytes before calling AppendUInt32ToContent
                    secondLastBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, secondLastBlock.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH) - 4);
                    AppendUInt32ToContent(secondLastBlock, lastBlock.Id);

                    // Forward 4 bytes, as an uint32 has been written
                    secondLastBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, secondLastBlock.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH) + 4);
                    secondLastBlock.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, 0);
                    lastBlock.SetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID, 0);

                    // Indicate success
                    return true;
                }
                // If this block is not empty then dequeue an UInt32 from it
                else
                {
                    blockId = ReadUInt32FromTrailingContent(lastBlock);
                    lastBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, currentBlockContentLength - 4);

                    // Indicate sucess
                    return true;
                }
            }
        }

        void AppendUInt32ToContent(IBlock block, uint value)
        {
            var contentLength = block.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH);

            if ((contentLength % 4) != 0)
            {
                throw new DataMisalignedException("Block content length not %4: " + contentLength);
            }

            block.Write(src: LittleEndianByteOrder.GetBytes(value), srcOffset: 0, dstOffset: (int)contentLength, count: 4);
        }

        uint ReadUInt32FromTrailingContent(IBlock block)
        {
            var buffer = new byte[4];
            var contentLength = block.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH);

            if ((contentLength % 4) != 0)
            {
                throw new DataMisalignedException("Block content length not %4: " + contentLength);
            }

            if (contentLength == 0)
            {
                throw new InvalidDataException("Trying to dequeue UInt32 from an empty block");
            }

            block.Read(dst: buffer, dstOffset: 0, srcOffset: (int)contentLength - 4, count: 4);
            return LittleEndianByteOrder.GetUInt32(buffer);
        }

        void MarkAsFree(uint blockId)
        {
            IBlock lastBlock, secondLastBlock, targetBlock = null;
            GetSpaceTrackingBlock(out lastBlock, out secondLastBlock);

            using (lastBlock)
            using (secondLastBlock)
            {
                try
                {
                    // Just append a number, if there is some space left
                    var contentLength = lastBlock.GetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH);
                    if ((contentLength + 4) <= _storage.BlockContentSize)
                    {
                        targetBlock = lastBlock;
                    }
                    // No more space left, allocate new block for writing.
                    // Note that we allocate fresh new block, if we reuse it may mess things up
                    else
                    {
                        targetBlock = _storage.CreateNew();
                        targetBlock.SetHeader(HEADER_FIELD_PREVIOUS_BLOCK_ID, lastBlock.Id);

                        lastBlock.SetHeader(HEADER_FIELD_NEXT_BLOCK_ID, targetBlock.Id);

                        contentLength = 0;
                    }

                    // Write!
                    AppendUInt32ToContent(targetBlock, blockId);

                    // Extend the block length to 4, as we wrote a number
                    targetBlock.SetHeader(HEADER_FIELD_BLOCK_CONTENT_LENGTH, contentLength + 4);
                }
                finally
                {
                    // Always dispose targetBlock
                    if (targetBlock != null)
                    {
                        targetBlock.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Get the last 2 blocks from the free space tracking record, 
        /// </summary>
        void GetSpaceTrackingBlock(out IBlock lastBlock, out IBlock secondLastBlock)
        {
            lastBlock = null;
            secondLastBlock = null;

            // Grab all record 0's blocks
            var blocks = FindBlocks(0);

            try
            {
                if (blocks == null || (blocks.Count == 0))
                {
                    throw new Exception("Failed to find blocks of record 0");
                }

                // Assign
                lastBlock = blocks[blocks.Count - 1];
                if (blocks.Count > 1)
                {
                    secondLastBlock = blocks[blocks.Count - 2];
                }
            }
            finally
            {
                // Awlays dispose unused blocks
                if (blocks != null)
                {
                    foreach (var block in blocks)
                    {
                        if ((lastBlock == null || block != lastBlock)
                            && (secondLastBlock == null || block != secondLastBlock))
                        {
                            block.Dispose();
                        }
                    }
                }
            }
        }
    }
}