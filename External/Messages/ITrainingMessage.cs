using AIkailo.Messaging;

namespace AIkailo.External
{
    public interface ITrainingMessage : IMessage
    {
        ISensorMessage SensorMessage { get; set; }
        IActionMessage ActionMessage { get; set; }
    }
}
