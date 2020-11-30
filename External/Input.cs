using System;
using AIkailo.External.Model;

namespace AIkailo.External
{
    public class Input
    {
        public string Name { get; }

        internal Action<string, FeatureVector> InputEvent { get; set; }

        public Input(string name)
        {
            Name = name;
        }

        public void OnInputEvent(FeatureVector data)
        {
            InputEvent?.Invoke(Name, data);
        }
    }
}
