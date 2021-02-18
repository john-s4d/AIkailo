using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.External.Common;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Common;

namespace AIkailo.Core
{
    internal class OutputMessageConsumer : IMessageConsumer<OutputMessage>
    {
        public async Task Consume(ConsumeContext<OutputMessage> context)
        {
            Console.WriteLine("OutputMessageConsumer.Consume(OutputMessage)");

            // Send the message to an external target
            var message = new ExternalMessage("Interaction.Output", new FeatureArray { { "output", "bar" } });

            ISendEndpoint endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://localhost/{message.Target}"));

            await endpoint.Send(message);

        }
    }
}
