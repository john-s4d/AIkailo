namespace AIkailo.Model
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
