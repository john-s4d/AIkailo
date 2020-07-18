namespace AIkailo.Messaging
{
    public interface IMessageConsumer<TMessage> 
        : MassTransit.IMessageConsumer<TMessage> where TMessage : class, IMessage
    { }
}
