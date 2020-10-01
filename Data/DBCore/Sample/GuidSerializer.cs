using System;
using AIkailo.Data.DBCore;

namespace AIkailo.Data.Sample
{
	/// <summary>
	/// Just a thin wrapper around Guid.ToByteArray() to make it compatible with ISerializer[Guid]
	/// </summary>
	public class GuidSerializer : ISerializer<Guid>
	{
		public byte[] Serialize (Guid value)
		{
			return value.ToByteArray ();
		}

		public Guid Deserialize (byte[] buffer, int offset, int length)
		{
			if (length != 16) {
				throw new ArgumentException (nameof(length));
			}

			return BufferHelper.ReadBufferGuid (buffer, offset);
		}

		public bool IsFixedSize {
			get {
				return true;
			}
		}
		public int Length {
			get {
				return 16;
			}
		}
	}
}

