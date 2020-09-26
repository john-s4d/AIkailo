namespace AIkailo.Model.Internal
{
    public class NormalizeMessage : IMessage
    {
        public Scene Scene { get; }

        public NormalizeMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
