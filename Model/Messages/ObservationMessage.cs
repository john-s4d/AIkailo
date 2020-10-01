namespace AIkailo.Model.Internal
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
