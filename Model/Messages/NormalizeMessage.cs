namespace AIkailo.Model.Internal
{
    public class NormalizeMessage : IMessage
    {
        public IScene Scene { get; }

        public NormalizeMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
