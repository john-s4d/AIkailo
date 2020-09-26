using System;
using AIkailo.Model.Internal;
using AIkailo.Messaging;
using System.Threading.Tasks;
using MassTransit;

namespace AIkailo.Core
{
    // Reduce the concepts in a scene to common parents
    internal class ReduceMessageConsumer : IMessageConsumer<ReduceMessage>
    {

        public Task Consume(ConsumeContext<ReduceMessage> context)
        {
            Console.WriteLine("ReduceMessageConsumer.Consume(ReduceMessage)");

            Scene scene = context.Message.Scene;

            /*
            foreach (Association a in scene)
            {
                Concept c = a.
            }

            AIkailo.DataService.
            */

            // Send to the Observation Service
            AIkailo.MessageService.Publish(new ObservationMessage(scene));
            return null;
        }
    }
}
