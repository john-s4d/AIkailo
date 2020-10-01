using System;
using AIkailo.Common;

namespace AIkailo.External
{
    public class Input
    {
        public string Name { get; }

        internal Action<string, DataPackage> InputEvent { get; set; }

        public Input(string name)
        {
            Name = name;
        }

        public void OnInputEvent(DataPackage data)
        {
            InputEvent?.Invoke(Name, data);
        }
    }
}
