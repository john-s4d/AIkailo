using System;
using AIkailo.External.Model;

namespace AIkailo.External
{
    public class AIkailoInput
    {
        public string Name { get; }

        internal Action<string, FeatureArray> InputEvent { get; set; }

        public AIkailoInput(string name)
        {
            Name = name;
        }

        public void OnInputEvent(FeatureArray data)
        {
            InputEvent?.Invoke(Name, data);
        }
    }
}
