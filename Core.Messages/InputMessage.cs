using AIkailo.External.Model;

namespace AIkailo.Messaging.Messages
{
    public class InputMessage : IMessage
    {   

        public FeatureVector Data { get; }
        public string Source { get; }

        public InputMessage(string source, FeatureVector data)
        {
            Data = data;
            Source = source;
        }
    }
}
