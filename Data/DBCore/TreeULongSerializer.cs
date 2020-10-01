using System;

namespace AIkailo.Data.DBCore
{
	public class TreeULongSerializer : ISerializer<ulong>
	{
		public byte[] Serialize(ulong value)
		{
			return LittleEndianByteOrder.GetBytes(value);
		}

		public ulong Deserialize(byte[] buffer, int offset, int length)
		{
			if (length != 8)
			{
				throw new ArgumentException("Invalid length: " + length);
			}

			return BufferHelper.ReadBufferUInt64(buffer, offset);
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

