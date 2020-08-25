using AIkailo.Model;

namespace AIkailo.Messaging
{
    public interface IMessageConsumer<TMessage> : MassTransit.IConsumer<TMessage>
        where TMessage : class, IMessage
    { }
}
