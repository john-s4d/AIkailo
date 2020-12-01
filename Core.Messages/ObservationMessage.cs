using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ObservationMessage : IMessage
    {
        public Scene Scene { get; set; }

        public ObservationMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
