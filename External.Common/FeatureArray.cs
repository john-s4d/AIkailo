using System;
using System.Collections.Generic;

namespace AIkailo.External.Common
{
    public class FeatureArray : List<Feature>
    {            
        public void Add(Property item1, Property item2)            
        {   
            Add(new Feature(item1, item2));
        }
    }
}
