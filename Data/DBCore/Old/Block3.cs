using System;
using System.IO;
using System.Collections.Generic;

namespace AIkailo.Data.DBCore
{
    public class Block3 //: IDisposable
    {
        public long Id { get; }

        private readonly Stream _stream;
        private bool _isDirty = false;
        
        public Block3(long id, Stream stream)
        {   
            Id = id;
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
        /*
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
            //if (_headerMap.ContainsKey(key)) { return _headerMap[key].Position; }

            // Records are sorted and should be relatively uniform (??) 
            // Interpolation search should be efficient            
            // This could be made more intelligent by checking known keys in the the headermap

            // Jump to where the record is expected to be
            //short recordNumber = Convert.ToInt16(
           //     Convert.ToUInt64(RecordCount) * (key - FirstKey) / (LastKey - FirstKey)
           //     );
            
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

        // Write data to anwhere in the block
        public void Write(byte[] src, int srcOffset, int dstOffset, int count)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(Block));
            }

            // Validate argument
            if (false == ((dstOffset >= 0) && ((dstOffset + count) <= _storage.BlockContentSize)))
            {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            if (false == ((srcOffset >= 0) && ((srcOffset + count) <= src.Length)))
            {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            // Write bytes that belong to the firstSector
            if ((_storage.BlockHeaderSize + dstOffset) < _storage.DiskSectorSize)
            {
                var thisWrite = Math.Min(count, _storage.DiskSectorSize - _storage.BlockHeaderSize - dstOffset);
                Buffer.BlockCopy(
                    src: src,
                    srcOffset: srcOffset,
                    dst: _firstSector,
                    dstOffset: _storage.BlockHeaderSize + dstOffset,
                    count: thisWrite
                    );
                _isFirstSectorDirty = true;
            }

            // Write bytes that do not belong to the firstSector
            if ((_storage.BlockHeaderSize + dstOffset + count) > _storage.DiskSectorSize)
            {
                // Move underlying stream to correct position ready for writting
                _stream.Position = (Id * _storage.BlockSize)
                    + Math.Max(_storage.DiskSectorSize, _storage.BlockHeaderSize + dstOffset);

                // Exclude bytes that have been written to the first sector
                var d = _storage.DiskSectorSize - (_storage.BlockHeaderSize + dstOffset);
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

