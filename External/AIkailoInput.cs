using System;
using System.Collections.Generic;
using AIkailo.External.Common;

namespace AIkailo.External
{
    public class AIkailoInputInterface
    {
        public string Name { get; }

        //internal Action<string, FeatureArray> InputEvent { get; set; }
        internal Action<string, Dictionary<ulong,float>> InputEvent { get; set; }

        public AIkailoInputInterface(string name)
        {
            Name = name;
        }

        public void Input(Dictionary<ulong, float> data)
        {
            InputEvent?.Invoke(Name, data);
        }
    }
}
