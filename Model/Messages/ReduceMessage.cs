namespace AIkailo.Model.Internal
{
    public class ReduceMessage : IMessage
    {
        public IScene Scene { get; set; }

        public ReduceMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
