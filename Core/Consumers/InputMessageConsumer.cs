using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;
using AIkailo.External.Model;


namespace AIkailo.Core
{
    // Assemble a Scene from the given data. Merge into current context.    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        private IDataProvider _dataProvider = AIkailo.DataService.DataProvider;

        public Task Consume(ConsumeContext<InputMessage> context)
        {
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            foreach (Feature data in context.Message.Data)
            {

                Node node = new Node() { NodeType = NodeType.INPUT };

                // If this is a float between 0-1, apply the value directly to the node.

                if (data.Item2.TypeCode == TypeCode.Single && data.Item2 >=0 && data.Item2 <= 1)
                {
                    node.Label = $"{context.Message.Source}.{data.Item1}"; // TODO: Sanitize,Escape
                    node.Value = data.Item2;
                }
                else
                {
                    node.Label = $"{context.Message.Source}.{data.Item1}.{data.Item2}"; // TODO: Sanitize,Escape
                    node.Value = 1;
                }
                
                _dataProvider.Load(ref node);

                AIkailo.ExecutiveService.Merge(node);
            }

            return Task.CompletedTask;
        }
    }
}