using System;
using System.Collections.Generic;

namespace AIkailo.External.Model
{
    public class TrainingModel : List<DataFlow>
    {
        public void Add(FeatureArray input, FeatureArray output)
        {
            Add(new DataFlow(input, output));
        }
        public void Add(FeatureArray input, FeatureArray output, string source, string target)
        {
            Add(new DataFlow()
            {
                Input = input,
                Output = output,
                Source = source,
                Target = target
            });
        }
    }
}
