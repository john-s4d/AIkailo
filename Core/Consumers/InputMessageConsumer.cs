using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Common;
using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Core
{
    // Merge Features into current context.    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        public Task Consume(ConsumeContext<InputMessage> context)
        {
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            throw new NotImplementedException();

            /*
            AIkailo.ExecutiveService.Input(context.Message.Source, context.Message.Data);

            
            List<INeuron> nodes = new List<INeuron>();

            foreach (Feature data in context.Message.Data)
            {
                nodes.Add(_nodeProvider.MergeInputNeuron(context.Message.Source, data));
            }
            
            AIkailo.ExecutiveService.Input(nodes);
            */

            return Task.CompletedTask;
        }
    }
}