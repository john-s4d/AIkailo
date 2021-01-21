using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class OutputMessage : IMessage
    {
        public Scene Scene { get; }

        public OutputMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
