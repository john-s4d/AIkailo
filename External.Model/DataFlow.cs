using System;
using System.Collections.Generic;

namespace AIkailo.External.Common
{
    public class TrainingStep
    {
        public FeatureArray Input { get; set; }
        public FeatureArray Output { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }

        public TrainingStep() { }

        public TrainingStep(FeatureArray input, FeatureArray output)            
        {
            Input = input;
            Output = output;
        }

        public TrainingStep(FeatureArray input, FeatureArray output, string source, string target)
            : this(input, output)
        {
            Source = source;
            Target = target;
        }
    }
}
