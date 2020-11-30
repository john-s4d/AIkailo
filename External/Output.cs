﻿using System;
using AIkailo.External.Model;
using AIkailo.Messaging.Messages;

namespace AIkailo.External
{
    public class Output
    {
        public string Name { get; }
        public Action<FeatureVector> OutputEvent { get; set; }
        internal ExternalMessageConsumer Consumer { get; }

        public Output(string name)
        {
            Name = name;
            Consumer = new ExternalMessageConsumer();
            Consumer.ConsumeEvent += Consume;
        }

        internal void Consume(ExternalMessage message)
        {
            //throw new NotImplementedException();
            OutputEvent?.Invoke(message.Data);
        }
    }
}
