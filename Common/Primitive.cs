using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AIkailo.Common
{
    [JsonObject]
    public class Primitive : IConvertible
    {
        public static implicit operator Primitive(byte[] value) => throw new InvalidCastException(); // TypeCode not available
        public static implicit operator Primitive(string value) => new Primitive(value);
        public static implicit operator Primitive(bool value) => new Primitive(value);
        public static implicit operator Primitive(char value) => new Primitive(value);
        public static implicit operator Primitive(sbyte value) => new Primitive(value);
        public static implicit operator Primitive(byte value) => new Primitive(value);
        public static implicit operator Primitive(short value) => new Primitive(value);
        public static implicit operator Primitive(ushort value) => new Primitive(value);
        public static implicit operator Primitive(int value) => new Primitive(value);
        public static implicit operator Primitive(uint value) => new Primitive(value);
        public static implicit operator Primitive(long value) => new Primitive(value);
        public static implicit operator Primitive(ulong value) => new Primitive(value);
        public static implicit operator Primitive(float value) => new Primitive(value);
        public static implicit operator Primitive(double value) => new Primitive(value);
        public static implicit operator Primitive(decimal value) => new Primitive(value);
        public static implicit operator Primitive(DateTime value) => new Primitive(value);

        public static implicit operator byte[](Primitive value) => value.Bytes;
        public static implicit operator string(Primitive value) => value.ToString();
        public static implicit operator bool(Primitive value) => value.ToBoolean();
        public static implicit operator char(Primitive value) => value.ToChar();
        public static implicit operator sbyte(Primitive value) => value.ToSByte();
        public static implicit operator byte(Primitive value) => value.ToByte();       
        public static implicit operator short(Primitive value) => value.ToInt16();        
        public static implicit operator ushort(Primitive value) => value.ToUInt16();        
        public static implicit operator int(Primitive value) => value.ToInt32();        
        public static implicit operator uint(Primitive value) => value.ToUInt32();        
        public static implicit operator long(Primitive value) => value.ToInt64();
        public static implicit operator ulong(Primitive value) => value.ToUInt64();        
        public static implicit operator float(Primitive value) => value.ToSingle();        
        public static implicit operator double(Primitive value) => value.ToDouble();        
        public static implicit operator decimal(Primitive value) => value.ToDecimal();        
        public static implicit operator DateTime(Primitive value) => value.ToDateTime();

        public byte[] Bytes { get; private set; }
        public TypeCode TypeCode { get; private set; }
        public IFormatProvider FormatProvider { get; set; }

        [JsonConstructor]
        public Primitive(byte[] Bytes, TypeCode TypeCode)
        {
            this.Bytes = Bytes;
            this.TypeCode = TypeCode;
        }

        public Primitive(IConvertible value)
            : this(GetBytes(value), value.GetTypeCode())
        { }

        private Primitive() { }

        private static byte[] GetBytes(IConvertible value, IFormatProvider provider = null)
        {
            switch (value.GetTypeCode())
            {
                // 0 Bytes
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

    }
}
