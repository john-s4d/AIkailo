namespace AIkailo.Model.Internal
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
