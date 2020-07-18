using AIkailo.Internal;
using AIkailo.Messaging;
using AIkailo.Data;
using System;

namespace AIkailo.Executive
{
    public class ClassifyMessageConsumer : IMessageConsumer<ClassifyMessage>
    {
        public void Consume(ClassifyMessage message)
        {
            Console.WriteLine("ClassifyMessageConsumer.Consume(ClassifyMessage)");

            // Classify the message and see if it can be 
            throw new NotImplementedException();
        }
    }
}
