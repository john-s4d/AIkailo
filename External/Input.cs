using System;
using System.Collections.Generic;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.External
{
    public class Input
    {
        public string Name { get; }
        internal Action<string, List<Tuple<IConvertible, IConvertible>>> InputEvent { get; set; }

        public Input(string name)
        {
            Name = name;
        }

        public void CreateInputEvent(List<Tuple<IConvertible, IConvertible>> data)
        {
            InputEvent?.Invoke(Name, data);
        }
    }
}
