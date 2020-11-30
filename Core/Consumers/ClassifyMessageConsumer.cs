using AIkailo.Messaging;
using MassTransit;
using System;
using System.Threading.Tasks;
using AIkailo.Core.Model;
using AIkailo.Messaging.Messages;

namespace AIkailo.Core
{   
    internal class ClassifyMessageConsumer : IMessageConsumer<ClassifyMessage>
    {
        public Task Consume(ConsumeContext<ClassifyMessage> context)
        {
            Console.WriteLine("ClassifyMessageConsumer.Consume(ClassifyMessage)");

            // Classify the message and see if it can be 
            throw new NotImplementedException();
        }
    }
}
