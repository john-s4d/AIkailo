using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;
using AIkailo.External.Model;
using AIkailo.Data;
using System.Collections.Generic;

namespace AIkailo.Core
{

    // Create a process-model link between the input and output scenes

    internal class TrainingMessageConsumer : IMessageConsumer<TrainingMessage>
    {
        private IDataProvider _dataProvider = AIkailo.DataService.DataProvider;

        public Task Consume(ConsumeContext<TrainingMessage> context)
        {
            Console.WriteLine("TrainingMessageConsumer.Consume(TrainingMessage)");


            // GetOrCreate Input Nodes. 
            InputMessage inputMessage = context.Message.Input;


            // GetOrCreate Output Nodes.
            ExternalMessage outputMessage = context.Message.Output;


            // GetOrCreate a Process Model. Create links.
            

            /*
            // Convert Input to Scene

            
            List<Scene> inputScenes = new List<Scene>();

            if (!string.IsNullOrEmpty(context.Message.Input.Source))
            {
                inputScenes.Add(_sceneProvider.New(Constants.SENDER_GUID, context.Message.Input.Source));
            }

            foreach (DataPair data in context.Message.Input.Data)
            {
                inputScenes.Add(_sceneProvider.New(data.Item1, data.Item2));
            }
            Scene inputScene = _sceneProvider.New(inputScenes.ToArray());

            // Convert Output to Scene     
            
            List<Scene> outputScenes = new List<Scene>();
            
            if (!string.IsNullOrEmpty(context.Message.Output.Target)) {
                _sceneProvider.New(Constants.TARGET_GUID, context.Message.Output.Target); 
            }

            foreach (DataPair data in context.Message.Output.Data)
            {
                outputScenes.Add(_sceneProvider.New(data.Item1, data.Item2));
            }
            Scene outputScene = _sceneProvider.New(outputScenes.ToArray());

            Scene linkedScenes = _sceneProvider.DataProvider.GetOrCreate(inputScene, outputScene);
            

            */
            return Task.CompletedTask;
        }
    }
}