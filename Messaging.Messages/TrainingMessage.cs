using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
{
    public class TrainingMessage : IMessage
    {
        public InputMessage Input { get; set; }
        public ExternalMessage Output { get; set; }
        
        // TODO: hints / process model

        public TrainingMessage(InputMessage input, ExternalMessage output)
        {
            Input = input;
            Output = output;
        }
    }
}
