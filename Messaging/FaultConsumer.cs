using System;
using System.Threading.Tasks;
using AIkailo.Messaging.Messages;
using MassTransit;

namespace AIkailo.Messaging
{   
    internal class FaultConsumer<TMessage> : MassTransit.IConsumer<Fault<TMessage>>
        where TMessage : class, IMessage
    {
        public Task Consume(ConsumeContext<Fault<TMessage>> context)
        {
            //Console.WriteLine(string.Format("FaultConsumer.Consume(ConsumeContext<Fault<{0}>>", typeof(TMessage).Name));
            throw new NotImplementedException();
        }
    }
}