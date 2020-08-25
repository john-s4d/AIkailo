using AIkailo.Messaging;
using AIkailo.Model;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.External
{    
    internal class ExternalMessageConsumer : IMessageConsumer<ExternalMessage>
    {
        internal Action<ExternalMessage> ConsumeEvent { get; set; }
        internal Action<List<Tuple<IConvertible, IConvertible>>> OutputEvent { get; set; }

        public Task Consume(ConsumeContext<ExternalMessage> context)
        {
            ConsumeEvent?.Invoke(context.Message);
            return null;
        }
    }
}