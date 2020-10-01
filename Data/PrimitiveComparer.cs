using System;
using System.Collections.Generic;
using AIkailo.Common;

namespace AIkailo.Data
{
    internal class PrimitiveComparer : IComparer<Primitive>
    {
        public int Compare(Primitive x, Primitive y)
        {
            // Compare bytes in order
            var len = Math.Min(x.Bytes.Length, y.Bytes.Length);
            for (var i = 0; i < len; i++)
            {
                var b = x.Bytes[i].CompareTo(y.Bytes[i]);
                if (b != 0)
                {
                    return b;
                }
            }
            
            // Compare lengths
            var l = x.Bytes.Length.CompareTo(y.Bytes.Length);
            if (l != 0)
            {
                return l;
            }

            // Compare TypeCodes
            return x.TypeCode.CompareTo(y.TypeCode);
        }
    }
}