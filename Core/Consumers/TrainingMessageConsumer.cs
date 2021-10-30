using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Common;
using AIkailo.External.Common;
using System.Collections.Generic;
using AIkailo.Executive;

namespace AIkailo.Core
{
    internal class TrainingMessageConsumer : IMessageConsumer<TrainingMessage>
    {
        public Task Consume(ConsumeContext<TrainingMessage> context)
        {
            Console.WriteLine("TrainingMessageConsumer.Consume(TrainingMessage)");

            string source = context.Message.Input.Source;
            Dictionary<ulong, float> input = context.Message.Input.Data;
            Dictionary<string, float> inputData = new Dictionary<string, float>();

            // Source is added as prefix to the neuron Id
            foreach (ulong key in input.Keys)
            {
                if (input[key] == 0) { continue; } // Skip empty values
                inputData.Add($"{source}:{key}", input[key]);
            }

            string target = context.Message.Output.Target;
            Dictionary<ulong, float> output = context.Message.Output.Data;
            Dictionary<string, float> outputData = new Dictionary<string, float>();

            // Target is added as prefix to the neuron Id
            foreach (ulong key in output.Keys)
            {
                if (output[key] == 0) { continue; } // Skip empty values
                outputData.Add($"{target}:{key}", output[key]);
            }

            AIkailo.ExecutiveService.PrimaryAgent.Train(inputData, outputData);

            return Task.CompletedTask;
        }
    }
}