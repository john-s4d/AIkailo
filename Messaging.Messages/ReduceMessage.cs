using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ReduceMessage : IMessage
    {
        public Scene Scene { get; set; }

        public ReduceMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
