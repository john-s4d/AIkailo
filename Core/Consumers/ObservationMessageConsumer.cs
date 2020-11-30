using AIkailo.Messaging.Messages;
using AIkailo.Messaging;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AIkailo.Core
{   
    internal class ObservationMessageConsumer : IMessageConsumer<ObservationMessage>
    {
   
        public Task Consume(ConsumeContext<ObservationMessage> context)
        {
            Console.WriteLine("ObservationMessageConsumer.Consume(ObservationMessage)");

            AIkailo.ObservationService.Merge(context.Message.Scene);
            return null;
        }
    }
}
