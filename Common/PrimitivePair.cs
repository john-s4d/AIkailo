using System;

namespace AIkailo.Common
{
    public class PrimitivePair : Tuple<Primitive, Primitive>
    {
        public PrimitivePair(Primitive item1, Primitive item2)
            : base(item1, item2)
        { }
    }
}
