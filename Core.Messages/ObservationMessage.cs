using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class ObservationMessage : IMessage
    {
        public IScene Scene { get; set; }

        public ObservationMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
