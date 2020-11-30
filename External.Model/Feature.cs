using System;
using System.Runtime.CompilerServices;

namespace AIkailo.External.Model
{
    public class Feature : Tuple<Property, Property>, IFeature
    {   
        public Feature(Property item1, Property item2)
            : base(item1, item2)
        { }
    }
}
