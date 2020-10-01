using System;
using System.IO;
using System.Collections.Generic;

namespace AIkailo.Data.DBCore
{
    public class AssociationBlock //: IDisposable
    {
        // TODO: Better type casting

        public const short BLOCK_SIZE = 4096;
        public const short BLOCK_HEADER_SIZE = 32;
        public const short PADDING = 2;
        public const short CURRENT_VERSION = 1;
        public const short DISK_SECTOR_SIZE = 512;

        private readonly Stream _stream;
        //private readonly BinaryReader _reader;
        //private readonly BinaryWriter _writer;
        private bool _isDirty = false;

        private bool _isDisposed = false;
        //public event EventHandler Disposed;

        public byte Version { get; private set; }
        public long BlockId { get; private set; }
        public short RecordCount { get; private set; }
        public ulong FirstKey { get; private set; }
        public ulong LastKey { get; private set; }

        private AssociationRecordHeader[] _recordHeaders;
        private Dictionary<ulong, AssociationRecordHeader> _headerMap;
        
        public AssociationBlock(long blockId, Stream stream)
        {   
            BlockId = blockId;
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));

            ReadBlockHeader();
        }

        private void ReadBlockHeader()
        {
            _stream.Position = Convert.ToInt64(BlockId * BLOCK_SIZE);

            // Version // 1 Byte
            Version = _stream.ExpectByte();

            // BlockId // 8 Bytes
            if (_stream.ExpectInt64() != BlockId) { throw new InvalidDataException(nameof(BlockId)); }

            // FirstKey // 8 Bytes
            FirstKey = _stream.ExpectUInt64();

            // LastKey // 8 Bytes
            LastKey = _stream.ExpectUInt64();

            // RecordCount // 2 Bytes
            RecordCount = _stream.ExpectInt16();

            _recordHeaders = new AssociationRecordHeader[RecordCount];
        }

        private AssociationRecordHeader GetRecordHeader(short recordNumber)
        {   
            if (_recordHeaders[recordNumber] != null) {
                return _recordHeaders[recordNumber];
            }

            _stream.Position = (BlockId * BLOCK_SIZE) +
                BLOCK_HEADER_SIZE + (recordNumber * AssociationRecordHeader.SIZE);

            AssociationRecordHeader header = new AssociationRecordHeader(
                key: _stream.ExpectUInt64(), // 8 Bytes
                position: _stream.ExpectInt16(), // 2 Bytes
                length: _stream.ExpectInt16(), // 2 Bytes
                continuesAtBlock:_stream.ExpectInt64() // 8 bytes        
            );

            _recordHeaders[recordNumber] = header;
            _headerMap.Add(header.Key, header);
            return header;
        }

        private short? GetRecordPosition(ulong key)
        {   
            if (_headerMap.ContainsKey(key)) { return _headerMap[key].Position; }

            // Records are sorted and should be relatively uniform (??) 
            // Interpolation search should be efficient            
            // This could be made more intelligent by checking known keys in the the headermap

            // Jump to where the record is expected to be
            short recordNumber = Convert.ToInt16(
                Convert.ToUInt64(RecordCount) * (key - FirstKey) / (LastKey - FirstKey)
                );
            
            while (recordNumber >= 0 && recordNumber < RecordCount)
            {
                AssociationRecordHeader header = GetRecordHeader(recordNumber);

                if (header.Key == key)
                {
                    return header.Position;
                }

                // Iterate up or down
                int increment = key > header.Key ? 1 : -1;
                recordNumber = (short)(recordNumber + increment);
            }
            throw new KeyNotFoundException(key.ToString());
        }

        // Read data from anwhere in the block
        public void Read(byte[] buffer, int offset, int count)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(AssociationBlock));
            }

            if (count + offset >= BLOCK_SIZE)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            _stream.Position = (BlockId * BLOCK_SIZE) + offset;
            _stream.Read(buffer, offset, count);

        }
        /*
        // Write data to anwhere in the block
        public void Write(byte[] src, int srcOffset, int dstOffset, int count)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(Block));
            }

            // Validate argument
            if (false == ((dstOffset >= 0) && ((dstOffset + count) <= (BLOCK_SIZE - BLOCK_HEADER_SIZE))))
            {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            if (false == ((srcOffset >= 0) && ((srcOffset + count) <= src.Length)))
            {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            // Write bytes that belong to the firstSector
            if ((BLOCK_HEADER_SIZE + dstOffset) < DISK_SECTOR_SIZE)
            {
                var thisWrite = Math.Min(count, DISK_SECTOR_SIZE - BLOCK_HEADER_SIZE - dstOffset);
                Buffer.BlockCopy(
                    src: src,
                    srcOffset: srcOffset,
                    dst: _firstSector,
                    dstOffset: BLOCK_HEADER_SIZE + dstOffset,
                    count: thisWrite
                    );
                _isFirstSectorDirty = true;
            }

            // Write bytes that do not belong to the firstSector
            if ((BLOCK_HEADER_SIZE + dstOffset + count) > DISK_SECTOR_SIZE)
            {
                // Move underlying stream to correct position ready for writting
                _stream.Position = ( BLOCK_SIZE)
                    + Math.Max(DISK_SECTOR_SIZE, BLOCK_HEADER_SIZE + dstOffset);

                // Exclude bytes that have been written to the first sector
                var d = DISK_SECTOR_SIZE - (BLOCK_HEADER_SIZE + dstOffset);
                if (d > 0)
                {
                    dstOffset += d;
                    srcOffset += d;
                    count -= d;
                }

                // Keep writing until all data is written
                var written = 0;
                while (written < count)
                {
                    var bytesToWrite = Math.Min(4096, count - written);
                    _stream.Write(src, srcOffset + written, bytesToWrite);
                    _stream.Flush();
                    written += bytesToWrite;
                }
            }
        }
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_isDirty)
                    {
                        //this.Write();
                        //_stream.Position = (BlockId * BLOCK_SIZE);
                        //_stream.Save();
                        //_stream.Flush();
                        //_isFirstSectorDirty = false;
                    }

                    OnDisposed(EventArgs.Empty);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AssociationBlock() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        /*
        public void Dispose()
        {
            _stream.Dispose();
        }

        
        //
        // Protected Methods
        //

        protected virtual void OnDisposed(EventArgs e)
        {
            Disposed?.Invoke(this, e);
        }

        //
        // Dispose
        //
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;

                if (_isFirstSectorDirty)
                {
                    _stream.Position = (Id * _storage.BlockSize);
                    _stream.Write(_firstSector, 0, 4096);
                    _stream.Flush();
                    _isFirstSectorDirty = false;
                }

                OnDisposed(EventArgs.Empty);
            }
        }

        ~AssociationBlock()
        {
            Dispose(false);
        }
        */
    }
}

