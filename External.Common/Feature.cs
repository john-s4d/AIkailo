using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace AIkailo.External.Common
{
    public class Feature : Tuple<Property, Property>
    {   
        [JsonConstructor]
        public Feature(Property item1, Property item2)
            : base(item1, item2)
        { }

        /*
        public Feature(Property label, float bias)
            : base(label, bias)
        { }
        */
    }
}
