using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace AIkailo.External.Model
{
    [JsonObject, Serializable]
    public class Property : IProperty
    {
        public static implicit operator Property(byte[] value) => throw new InvalidCastException(); // TypeCode not available. TODO: treat as serialized?
        public static implicit operator Property(string value) => new Property(value);
        public static implicit operator Property(bool value) => new Property(value);
        public static implicit operator Property(char value) => new Property(value);
        public static implicit operator Property(sbyte value) => new Property(value);
        public static implicit operator Property(byte value) => new Property(value);
        public static implicit operator Property(short value) => new Property(value);
        public static implicit operator Property(ushort value) => new Property(value);
        public static implicit operator Property(int value) => new Property(value);
        public static implicit operator Property(uint value) => new Property(value);
        public static implicit operator Property(long value) => new Property(value);
        public static implicit operator Property(ulong value) => new Property(value);
        public static implicit operator Property(float value) => new Property(value);
        public static implicit operator Property(double value) => new Property(value);
        public static implicit operator Property(decimal value) => new Property(value);
        public static implicit operator Property(DateTime value) => new Property(value);

        public static implicit operator byte[](Property value) => value.Bytes;
        public static implicit operator string(Property value) => value.ToString();
        public static implicit operator bool(Property value) => value.ToBoolean();
        public static implicit operator char(Property value) => value.ToChar();
        public static implicit operator sbyte(Property value) => value.ToSByte();
        public static implicit operator byte(Property value) => value.ToByte();
        public static implicit operator short(Property value) => value.ToInt16();
        public static implicit operator ushort(Property value) => value.ToUInt16();
        public static implicit operator int(Property value) => value.ToInt32();
        public static implicit operator uint(Property value) => value.ToUInt32();
        public static implicit operator long(Property value) => value.ToInt64();
        public static implicit operator ulong(Property value) => value.ToUInt64();
        public static implicit operator float(Property value) => value.ToSingle();
        public static implicit operator double(Property value) => value.ToDouble();
        public static implicit operator decimal(Property value) => value.ToDecimal();
        public static implicit operator DateTime(Property value) => value.ToDateTime();

        public byte[] Bytes { get; private set; }
        public TypeCode TypeCode { get; private set; }

        // TODO: Serialize more efficiently. Binary or Base64 for transport.
        [JsonConstructor]
        public Property(byte[] Bytes, TypeCode? typeCode = TypeCode.Empty)
        {
            this.Bytes = Bytes;
            this.TypeCode = typeCode ?? TypeCode.Empty;
        }

        public Property(IConvertible value)
            : this(GetBytes(value), value?.GetTypeCode())
        { }

        public Property() { }


        public Property(SerializationInfo info, StreamingContext context)
            : this(
                  (byte[])info.GetValue("Bytes", typeof(byte[])),
                  (TypeCode)info.GetValue("TypeCode", typeof(TypeCode))
                  )
        { }

        private static byte[] GetBytes(IConvertible value, IFormatProvider provider = null)
        {
            switch (value?.GetTypeCode())
            {
                // 0 Bytes
                case null:
                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.Object:
                    return new byte[0];

                // 1 Byte
                case TypeCode.Byte:
                    return new byte[] { value.ToByte(provider) };
                case TypeCode.SByte:
                    return new byte[] { Convert.ToByte(value.ToSByte(provider)) };
                case TypeCode.Boolean:
                    return BitConverter.GetBytes(value.ToBoolean(provider));

                // 2 Bytes
                case TypeCode.Char:
                    return BitConverter.GetBytes(value.ToChar(provider));
                case TypeCode.Int16:
                    return BitConverter.GetBytes(value.ToInt16(provider));
                case TypeCode.UInt16:
                    return BitConverter.GetBytes(value.ToUInt16(provider));

                // 4 Bytes
                case TypeCode.Single:
                    return BitConverter.GetBytes(value.ToSingle(provider));
                case TypeCode.Int32:
                    return BitConverter.GetBytes(value.ToInt32(provider));
                case TypeCode.UInt32:
                    return BitConverter.GetBytes(value.ToUInt32(provider));

                // 8 Bytes                
                case TypeCode.Double:
                    return BitConverter.GetBytes(value.ToDouble(provider));
                case TypeCode.Int64:
                    return BitConverter.GetBytes(value.ToInt64(provider));
                case TypeCode.UInt64:
                    return BitConverter.GetBytes(value.ToUInt64(provider));
                case TypeCode.DateTime:
                    return BitConverter.GetBytes(value.ToDateTime(provider).Ticks);

                // 24 Bytes
                case TypeCode.Decimal:
                    return decimal.GetBits(value.ToDecimal(provider)).Select(x => (byte)x).ToArray();

                // n Bytes
                case TypeCode.String:
                    return Encoding.Unicode.GetBytes(value.ToString());

                default:
                    throw new InvalidCastException();
            }
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(IFormatProvider provider)
        {
            return Encoding.Unicode.GetString(Bytes);
        }

        public bool ToBoolean(IFormatProvider provider = null)
        {
            return BitConverter.ToBoolean(Bytes, 0);
        }

        public char ToChar(IFormatProvider provider = null)
        {
            return Encoding.Unicode.GetChars(Bytes)[0];
        }

        public sbyte ToSByte(IFormatProvider provider = null)
        {
            return Convert.ToSByte(Bytes[0]);
        }

        public byte ToByte(IFormatProvider provider = null)
        {
            return Bytes[0];
        }

        public short ToInt16(IFormatProvider provider = null)
        {
            return Convert.ToInt16(Bytes[0]);
        }

        public ushort ToUInt16(IFormatProvider provider = null)
        {
            return BitConverter.ToUInt16(Bytes, 0);
        }

        public int ToInt32(IFormatProvider provider = null)
        {
            return BitConverter.ToInt32(Bytes, 0);
        }

        public uint ToUInt32(IFormatProvider provider = null)
        {
            return BitConverter.ToUInt32(Bytes, 0);
        }

        public long ToInt64(IFormatProvider provider = null)
        {
            return BitConverter.ToInt64(Bytes, 0);
        }

        public ulong ToUInt64(IFormatProvider provider = null)
        {
            return BitConverter.ToUInt64(Bytes, 0);
        }

        public float ToSingle(IFormatProvider provider = null)
        {
            return BitConverter.ToSingle(Bytes, 0);
        }

        public double ToDouble(IFormatProvider provider = null)
        {
            return BitConverter.ToDouble(Bytes, 0);
        }

        public decimal ToDecimal(IFormatProvider provider = null)
        {
            int[] parts = Bytes.Select(x => (int)x).ToArray();
            return new decimal(
                parts[0],
                parts[1],
                parts[2],
                (parts[3] & 0x80000000) != 0,
                (byte)((parts[3] >> 16) & 0x7F)
            );
        }

        public DateTime ToDateTime(IFormatProvider provider = null)
        {
            return new DateTime(ToInt64());
        }

        public object ToType(Type conversionType, IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        public TypeCode GetTypeCode()
        {
            return this.TypeCode;
        }

        public bool Equals(Property obj)
        {
            return
                obj != null &&
                TypeCode.Equals(obj.TypeCode) &&
                StructuralComparisons.StructuralEqualityComparer.Equals(Bytes, obj.Bytes);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Property);
        }

        public override int GetHashCode()
        {
            return (TypeCode, Bytes).GetHashCode();
        }

        // FIXME: Use TypeCode's compare, rather than bytes. To ensure they are always in expected order.
        public int CompareTo(Property other)
        {
            if (other == null) { return 1; }

            // Compare bytes in order
            var len = Math.Min(Bytes.Length, other.Bytes.Length);
            for (var i = 0; i < len; i++)
            {
                var b = Bytes[i].CompareTo(other.Bytes[i]);
                if (b != 0)
                {
                    return b;
                }
            }

            // Compare lengths and type
            var bCompare = Bytes.Length.CompareTo(other.Bytes.Length);
            return bCompare == 0 ? TypeCode.CompareTo(other.TypeCode) : bCompare;

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Bytes", Bytes, typeof(byte[]));
            info.AddValue("TypeCode", TypeCode, typeof(TypeCode));
        }

        /*
        internal class PrimitiveSerializer //: ISerializer<Primitive>
        {
            public PrimitiveSerializer() { }

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
        */
    }
}
