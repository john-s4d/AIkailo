using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ClassifyMessage : IMessage
    {
        public IScene Scene { get; set; }
    }
}
