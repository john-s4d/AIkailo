using AIkailo.External.Model;

namespace AIkailo.Messaging.Messages
{
    public class ExternalMessage : IMessage
    {
        public FeatureVector Data { get; }

        public ExternalMessage(FeatureVector data) 
        {
            Data = data;
        }
    }
}
