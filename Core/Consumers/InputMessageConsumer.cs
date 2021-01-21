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
    // Assemble a Scene from the given data. Merge into current context.    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        private ISceneProvider _sceneProvider = AIkailo.DataService.SceneProvider;

        public Task Consume(ConsumeContext<InputMessage> context)
        {
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            List<Scene> sceneParts = new List<Scene> { _sceneProvider.New(Constants.SENDER_GUID, context.Message.Source) };

            foreach (Feature data in context.Message.Data)
            {
                sceneParts.Add(_sceneProvider.New(data.Item1, data.Item2));
            }
                        
            return AIkailo.ExecutiveService.Merge(_sceneProvider.New(sceneParts.ToArray()));            
        }
    }
}