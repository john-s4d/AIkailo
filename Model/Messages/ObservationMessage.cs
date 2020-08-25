namespace AIkailo.Model
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
