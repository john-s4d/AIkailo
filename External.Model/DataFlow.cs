using System;
using System.Collections.Generic;

namespace AIkailo.External.Model
{
    public class DataFlow
    {
        public FeatureArray Input { get; set; }
        public FeatureArray Output { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }

        public DataFlow() { }

        public DataFlow(FeatureArray input, FeatureArray output)            
        {
            Input = input;
            Output = output;
        }

        public DataFlow(FeatureArray input, FeatureArray output, string source, string target)
            : this(input, output)
        {
            Source = source;
            Target = target;
        }
    }
}
