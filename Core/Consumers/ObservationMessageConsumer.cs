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

            //return AIkailo.ExecutiveService.Merge(context.Message.Scene);            
            throw new NotImplementedException();
        }
    }
}
