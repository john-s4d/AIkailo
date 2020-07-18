using AIkailo.Messaging;

namespace AIkailo.Internal
{
    public interface ISceneMessage : IMessage
    {
        Scene Scene { get; set; }
    }
}
