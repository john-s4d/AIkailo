using AIkailo.Internal;
using AIkailo.Messaging;
using System;

namespace AIkailo.Data
{
    public class ReduceMessageConsumer : IMessageConsumer<ReduceMessage>
    {
        public void Consume(ReduceMessage message)
        {
            Console.WriteLine("ReduceMessageConsumer.Consume(ReduceMessage)");
            throw new NotImplementedException();

            /*
            // Reduce the scene and pass it to the classifier
            DataService.Publish(new ClassifyMessage()
            {
                Scene = DataService.Data.Reduce(message.Scene)
            });
            
            
            // **** ECHO TEST SHORT-CIRCUIT *****
            // Reduce the scene and pass it to the expander            
            DataService.Publish(new ExpandMessage()
            {
                Scene = DataService.Data.Reduce(message.Scene)
            });*/
        }
    }
}
