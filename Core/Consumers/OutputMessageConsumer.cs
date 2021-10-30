using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.External.Common;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Common;
using System.Collections.Generic;

namespace AIkailo.Core
{
    internal class OutputMessageConsumer : IMessageConsumer<OutputMessage>
    {
        public async Task Consume(ConsumeContext<OutputMessage> context)
        {
            Console.WriteLine("OutputMessageConsumer.Consume(OutputMessage)");

            // Send the message to an external target

            //ISendEndpoint endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://localhost/{message.Target}"));

            //await endpoint.Send(message);

            throw new NotImplementedException();

        }
    }
}
