using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.External.Model;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;

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

        public static Node CreateOutputNode(string target, Feature data)
        {
            var node = new Node() { NodeType = NodeType.OUTPUT };
            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between 0-1, apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= 0 && value <= 1)
            {
                node.Label = $"{target}.{label}"; // TODO: Sanitize,Escape
                node.Value = value;
            }
            else
            {
                node.Label = $"{target}.{label}.{value}"; // TODO: Sanitize,Escape
                node.Value = 1;
            }

            return node;
        }
    }
}
