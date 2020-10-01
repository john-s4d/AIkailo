using AIkailo.Common;
using AIkailo.Data.DBCore;
using System;

namespace AIkailo.Data
{
    internal class PrimitiveSerializer : ISerializer<Primitive>
    {
        public bool IsFixedSize { get; set; } = false;

        public int Length => throw new InvalidOperationException();

        public Primitive Deserialize(byte[] buffer, int offset, int length = 0)
        {
            var typeCode = BufferHelper.ReadBufferInt32(buffer, 0);
            var data = BufferHelper.ReadBufferOffset(buffer, 4, buffer.Length - 4);
            return new Primitive(data, (TypeCode)typeCode);
        }

        public byte[] Serialize(Primitive value)
        {
            var buffer = new byte[4 + value.Bytes.Length];
            BufferHelper.WriteBuffer((int)value.TypeCode, buffer, 0);
            Buffer.BlockCopy(value.Bytes, 0, buffer, 4, value.Bytes.Length);
            return buffer;
        }
    }
}