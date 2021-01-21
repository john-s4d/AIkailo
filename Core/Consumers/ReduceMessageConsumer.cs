using System;
using AIkailo.Core.Model;
using AIkailo.Messaging;
using System.Threading.Tasks;
using MassTransit;
using AIkailo.Messaging.Messages;

namespace AIkailo.Core
{
    // Reduce the concepts in a scene to common parents
    internal class ReduceMessageConsumer : IMessageConsumer<ReduceMessage>
    {

        public Task Consume(ConsumeContext<ReduceMessage> context)
        {
            Console.WriteLine("ReduceMessageConsumer.Consume(ReduceMessage)");

            //IScene scene = context.Message.Scene;

            // Send to the Observation Service
            //AIkailo.MessageService.Publish(new ObservationMessage(scene));
            return null;
        }
    }
}
