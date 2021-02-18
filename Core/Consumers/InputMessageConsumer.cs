using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Common;
using AIkailo.External.Common;
using System.Collections.Generic;

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
                Node node = _nodeFactory.MergeInputNode(context.Message.Source, data);
                //_nodeFactory.Load(ref node);
                nodes.Add(node);

                // TODO: Implement batching so it can be returned as one task
                AIkailo.ExecutiveService.Incoming(node);
            }
            
            return Task.CompletedTask;
        }
    }
}