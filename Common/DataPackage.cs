using System;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public class DataPackage : List<PrimitivePair>
    {            
        public void Add(Primitive item1, Primitive item2)            
        {   
            Add(new PrimitivePair(item1, item2));
        }
    }
}
