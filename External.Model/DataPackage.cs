using System;
using System.Collections.Generic;

namespace AIkailo.Model.Common
{
    public class DataPackage : List<PrimitivePair>
    {            
        public void Add(Primitive item1, Primitive item2)            
        {   
            Add(new PrimitivePair(item1, item2));
        }

        //public static implicit operator Model.DataPackage(External.DataPackage data) => new DataPackage() { data.ToArray() };
    }
}
