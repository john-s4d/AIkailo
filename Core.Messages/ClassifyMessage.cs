using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ClassifyMessage : IMessage
    {
        public Scene Scene { get; set; }
    }
}
