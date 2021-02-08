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
    // Create a link between the input and output scenes

    internal class TrainingMessageConsumer : IMessageConsumer<TrainingMessage>
    {
        private IDataProvider _dataProvider = AIkailo.DataService.DataProvider;

        public Task Consume(ConsumeContext<TrainingMessage> context)
        {
            Console.WriteLine("TrainingMessageConsumer.Consume(TrainingMessage)");

            List<Node> inputNodes = new List<Node>();

            foreach (Feature data in context.Message.Input.Data)
            {
                var inputNode = InputMessageConsumer.CreateInputNode(context.Message.Input.Source, data);
                _dataProvider.Load(ref inputNode);
                inputNodes.Add(inputNode);                
            }

            List<Node> outputNodes = new List<Node>();

            foreach (Feature data in context.Message.Output.Data)
            {
                var outputNode = OutputMessageConsumer.CreateOutputNode(context.Message.Output.Target, data);
                _dataProvider.Load(ref outputNode);
                outputNodes.Add(outputNode);
            }

            return AIkailo.ExecutiveService.Train(inputNodes, outputNodes);
            
        }
    }
}