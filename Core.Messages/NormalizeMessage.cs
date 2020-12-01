using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class NormalizeMessage : IMessage
    {
        public Scene Scene { get; }

        public NormalizeMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
