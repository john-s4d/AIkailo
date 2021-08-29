using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Common;
using AIkailo.External.Common;
using System.Collections.Generic;
using AIkailo.Executive;
using AIkailo.Neural.Core;

namespace AIkailo.Core
{
    // Create a hint between the input and output nodes

    internal class TrainingMessageConsumer : IMessageConsumer<TrainingMessage>
    {
        private INodeProvider _nodeProvider = AIkailo.DataService.NodeProvider;
        private Trainer _trainer = AIkailo.ExecutiveService.Trainer;

        public Task Consume(ConsumeContext<TrainingMessage> context)
        {
            Console.WriteLine("TrainingMessageConsumer.Consume(TrainingMessage)");

            List<Node> inputNodes = new List<Node>();
            List<Node> outputNodes = new List<Node>();

            foreach (Feature data in context.Message.Input.Data)
            {
                inputNodes.Add(_nodeProvider.MergeInputNode(context.Message.Input.Source, data));
            }

            foreach (Feature data in context.Message.Output.Data)
            {
                outputNodes.Add(_nodeProvider.MergeOutputNode(context.Message.Output.Target, data));
            }

            _trainer.MergeHint(inputNodes, outputNodes);

            return Task.CompletedTask;
        }
    }
}