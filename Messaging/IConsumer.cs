using AIkailo.Messaging.Messages;

namespace AIkailo.Messaging
{
    public interface IConsumer<TMessage> : MassTransit.IConsumer<TMessage>
        where TMessage : class, IMessage
    { }
}
