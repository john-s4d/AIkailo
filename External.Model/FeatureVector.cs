using System;
using System.Collections.Generic;

namespace AIkailo.External.Model
{
    public class FeatureVector : List<Feature>, IFeatureVector
    {            
        public void Add(Property item1, Property item2)            
        {   
            Add(new Feature(item1, item2));
        }
    }
}
