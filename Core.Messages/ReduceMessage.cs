using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ReduceMessage : IMessage
    {
        public IScene Scene { get; set; }

        public ReduceMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
