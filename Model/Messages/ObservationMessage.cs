namespace AIkailo.Model.Internal
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
