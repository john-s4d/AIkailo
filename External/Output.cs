using System;
using System.Collections.Generic;
using AIkailo.Model;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.External
{
    public class Output
    {
        public string Name { get; }
        public Action<List<Tuple<IConvertible, IConvertible>>> OutputEvent { get; set; }
        internal ExternalMessageConsumer Consumer { get; }

        public Output(string name)
        {
            Name = name;
            Consumer = new ExternalMessageConsumer();
            Consumer.ConsumeEvent += Consume;
        }

        internal void Consume(ExternalMessage message)
        {
            OutputEvent?.Invoke(message.Data);
        }
    }
}
