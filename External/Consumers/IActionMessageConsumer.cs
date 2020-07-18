using AIkailo.Messaging;

namespace AIkailo.External
{
    public interface IActionMessageConsumer<TMessage> 
        : IMessageConsumer<TMessage> where TMessage : class, IActionMessage, IMessage
    {
    }
}
