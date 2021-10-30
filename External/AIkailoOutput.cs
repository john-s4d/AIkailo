using System;
using System.Collections.Generic;
using AIkailo.External.Common;
using AIkailo.Messaging.Messages;

namespace AIkailo.External
{
    public class AIkailoOutput
    {
        public string Name { get; }
        //public Action<FeatureArray> OutputEvent { get; set; }
        public Action<Dictionary<ulong,float>> OutputEvent { get; set; }
        internal ExternalMessageConsumer Consumer { get; }

        public AIkailoOutput(string name)
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
