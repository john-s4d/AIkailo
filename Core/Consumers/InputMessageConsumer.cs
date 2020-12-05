using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using MassTransit;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;
using AIkailo.External.Model;
using AIkailo.Data;


namespace AIkailo.Core
{
    // Assemble a Scene from the given data    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        private ISceneProvider _sceneProvider = AIkailo.SceneProvider;
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
            //result.Add(scene1);
            //result.Add(scene2);

            
            foreach (PrimitivePair parameter in context.Message.Data)
            {
                result.Add(
                    AIkailo.DataService.FindOrCreate(parameter.Item1, parameter.Item2)                    
                );
            }

            // Send to the Reducer
            //AIkailo.MessageService.Publish(new ReduceMessage(result));
            */
            /*
            

            InputMessage input = context.Message;

            List<IConcept> parameters = new List<IConcept>();

            foreach (PrimitivePair parameter in context.Message.Data)
            {
                parameters.Add(new Scene(ds.GetOrCreate(parameter.Item1).Result, ds.GetOrCreate(parameter.Item2).Result));
            }
            
            context.Publish(new OutputMessage(Result));

            // TESTING - Just send a dummy OutputMessage
            
            Data.DataService ds = AIkailo.DataService;

            Task<IScene> task = ds.GetOrCreate(
                new Primitive[] { "target", "Interaction.Output" },
                new Primitive[] { "output", "Interaction.Output" }
                );
            */


            Scene s1 = _sceneProvider.New(
                _sceneProvider.New("target", "Interaction.Output"),
                _sceneProvider.New("output", "bar")
            );
            return context.Publish(s1);
        }
    }
}