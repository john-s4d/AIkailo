using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class NormalizeMessage : IMessage
    {
        public IScene Scene { get; }

        public NormalizeMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
