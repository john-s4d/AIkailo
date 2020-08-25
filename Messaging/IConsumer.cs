using AIkailo.Model;

namespace AIkailo.Messaging
{
    public interface IConsumer<TMessage> : MassTransit.IConsumer<TMessage>
        where TMessage : class, IMessage
    { }
}
