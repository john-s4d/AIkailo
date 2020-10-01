using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.Common;
using AIkailo.Model.Internal;
using MassTransit;

namespace AIkailo.Core
{
    // Assemble a Scene from the given data    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        public Task Consume(ConsumeContext<InputMessage> context)
        {

            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");

            /*
            Scene scene1 = new Scene
            {
                new Concept() { Definition = Constants.TARGET_GUID, Id = 0 },
                new Concept() { Definition = "Interaction.Output", Id = 1}
                //AIkailo.DataService.FindOrCreate(Constants.SENDER_GUID, context.Message.Source)
            };            
            scene1.Id = 2;

            Scene scene2 = new Scene
            {
                new Concept() {Definition = "output", Id = 3},
                new Concept() {Definition = context.Message.Data[0].Item2, Id = 4}
            };
            scene2.Id = 5;

            Scene result = new Scene();
            result.Add(scene1);
            result.Add(scene2);

            
            foreach (PrimitivePair parameter in context.Message.Data)
            {
                result.Add(
                    AIkailo.DataService.FindOrCreate(parameter.Item1, parameter.Item2)                    
                );
            }

            // Send to the Reducer
            //AIkailo.MessageService.Publish(new ReduceMessage(result));
            */

            InputMessage input = context.Message;

            // TESTING - Just send a dummy OutputMessage

            IScene result = AIkailo.DataService.FindOrCreate(
                AIkailo.DataService.FindOrCreate("target","Interaction.Output"),
                AIkailo.DataService.FindOrCreate("output","bar")
                );            

            return context.Publish(new OutputMessage(result));            
        }

    }
}