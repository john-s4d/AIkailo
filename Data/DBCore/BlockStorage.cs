using System;
using System.Collections.Generic;
using System.IO;

namespace AIkailo.Data.DBCore
{
    public class BlockStorage : IBlockStorage
    {
        public int DiskSectorSize { get; }
        public int BlockSize { get; }
        public int BlockHeaderSize { get; }
        public int BlockContentSize { get; }

        private readonly Stream _stream;

        private BlockCache _blockCache;
        private const uint CACHE_MAX_ITEMS = 0;
        private const uint CACHE_MAX_AGE = 0;
        private const uint CACHE_SWEEP_INTERVAL = 0;

        private readonly Dictionary<uint, Block> _blocks = new Dictionary<uint, Block>();

        public BlockStorage(Stream stream, int blockSize, int blockHeaderSize, int diskSectorSize)
        {
            if (blockSize < diskSectorSize || blockSize % diskSectorSize != 0)
            {
                throw new ArgumentException(nameof(blockSize));
            }

            if (blockHeaderSize >= blockSize) { throw new ArgumentException(nameof(blockHeaderSize)); }

            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            BlockSize = blockSize;
            BlockHeaderSize = blockHeaderSize;
            DiskSectorSize = diskSectorSize;
            BlockContentSize = blockSize - blockHeaderSize;

            _blockCache = new BlockCache(CACHE_MAX_ITEMS, CACHE_MAX_AGE, CACHE_SWEEP_INTERVAL);
        }

        public IBlock Find(uint blockId)
        {
            long blockPosition = blockId * BlockSize;

            if ((blockPosition + BlockSize) > _stream.Length)
            {
                return null;
            }
            
            //Read the first sector to get the headers
            var firstSector = new byte[DiskSectorSize];
            _stream.Position = blockPosition;
            _stream.Read(firstSector, 0, DiskSectorSize);

            var block = new Block(this, blockId, firstSector, _stream);
            OnBlockInitialized(block);
            return block;
        }

        public IBlock CreateNew()
        {
            if (_stream.Length % BlockSize != 0)
            {
                throw new DataMisalignedException("Unexpected length of the stream: " + _stream.Length);
            }

            // Calculate new block id
            var blockId = (uint)Math.Ceiling((double)_stream.Length / (double)BlockSize);

            // Extend length of underlying stream
            _stream.Flush();
            _stream.SetLength((blockId * BlockSize) + BlockSize);

            // Return desired block
            var block = new Block(this, blockId, new byte[DiskSectorSize], _stream);
            OnBlockInitialized(block);
            return block;
        }

        //
        // Protected Methods
        //

        protected virtual void OnBlockInitialized(Block block)
        {
            // Keep reference to it
            //_blocks[block.Id] = block;

            // When block disposed, remove it from memory
            block.Disposed += HandleBlockDisposed;
        }

        protected virtual void HandleBlockDisposed(object sender, EventArgs e)
        {
            // Stop listening to it
            ((Block)sender).Disposed -= HandleBlockDisposed;

            // Remove it from memory
            //_blocks.Remove(block.Id);
        }
    }
}
