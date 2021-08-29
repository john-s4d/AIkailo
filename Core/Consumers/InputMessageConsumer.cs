using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Common;
using AIkailo.External.Common;
using System.Collections.Generic;
using AIkailo.Neural.Core;

namespace AIkailo.Core
{
    // Convert features to nodes. Merge into current context.    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        private INodeProvider _nodeFactory = AIkailo.DataService.NodeProvider;
        

        public Task Consume(ConsumeContext<InputMessage> context)
        {
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            List<Node> nodes = new List<Node>();

            foreach (Feature data in context.Message.Data)
            {
                nodes.Add(_nodeFactory.MergeInputNode(context.Message.Source, data));
            }
            
            AIkailo.ExecutiveService.Incoming(nodes);
            return Task.CompletedTask;
        }
    }
}