using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;
using AIkailo.External.Model;
using System.Collections.Generic;

namespace AIkailo.Core
{
    // Assemble a Scene from the given data. Merge into current context.    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        private IDataProvider _dataProvider = AIkailo.DataService.DataProvider;

        public Task Consume(ConsumeContext<InputMessage> context)
        {
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            List<Node> nodes = new List<Node>();

            foreach (Feature data in context.Message.Data)
            {
                Node node = CreateInputNode(context.Message.Source, data);                
                _dataProvider.Load(ref node);
                nodes.Add(node);

                // TODO: Batch so it can be returned as one task
                AIkailo.ExecutiveService.Incoming(node);
            }
            
            return Task.CompletedTask;
        }

        public static Node CreateInputNode(string source, Feature data)
        {
            var node = new Node() { NodeType = NodeType.INPUT };
            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between 0-1, apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= 0 && value <= 1)
            {
                node.Label = $"{source}.{label}"; // TODO: Sanitize,Escape
                node.Value = value;
            }
            else
            {
                node.Label = $"{source}.{label}.{value}"; // TODO: Sanitize,Escape
                node.Value = 1;
            }

            return node;
        }
    }
}