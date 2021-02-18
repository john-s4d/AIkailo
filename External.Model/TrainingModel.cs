using System;
using System.Collections.Generic;

namespace AIkailo.External.Common
{
    public class TrainingModel : List<TrainingStep>
    {
        public void Add(FeatureArray input, FeatureArray output)
        {
            Add(new TrainingStep(input, output));
        }
        public void Add(FeatureArray input, FeatureArray output, string source, string target)
        {
            Add(new TrainingStep()
            {
                Input = input,
                Output = output,
                Source = source,
                Target = target
            });
        }
    }
}
