using System;

namespace AIkailo.Data.DBCore
{
    public class TreeLongSerializer : ISerializer<long>
    {
        public byte[] Serialize(long value)
        {
            return LittleEndianByteOrder.GetBytes(value);
        }

        public long Deserialize(byte[] buffer, int offset = 0, int length = 8)
        {
            if (length != 8)
            {
                throw new ArgumentException("Invalid length: " + length);
            }

            return BufferHelper.ReadBufferInt64(buffer, offset);
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public int Length
        {
            get
            {
                return 8;
            }
        }
    }
}

