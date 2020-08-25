using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.Model;
using MassTransit;

namespace AIkailo.Core
{
    // Assemble a Scene from the given data    
    internal class InputMessageConsumer : IMessageConsumer<InputMessage>
    {
        public Task Consume(ConsumeContext<InputMessage> context)            
        {
            throw new NotImplementedException();
            /*
            Console.WriteLine("InputMessageConsumer.Consume(InputMessage)");
            
            Scene result = new Scene
            {   
                AIkailo.DataService.FindOrCreate(Constants.SENDER_GUID, context.Message.Sender)
            };
            
            foreach (Tuple<IConvertible, IConvertible> parameter in context.Message.Data)
            {
                result.Add(
                    AIkailo.DataService.FindOrCreate(parameter.Item1, parameter.Item2)                    
                );
            }

            // Send to the Reducer
            //AIkailo.MessageService.Publish(new ReduceMessage(result));

            // SHORTCUT - Send directly to output
            AIkailo.MessageService.Publish(new OutputMessage(result));*/
            return null;
        }

    }
}