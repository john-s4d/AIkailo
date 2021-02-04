using AIkailo.External.Model;

namespace AIkailo.Messaging.Messages
{
    public class InputMessage : IMessage
    {
        public string Source { get; set; }
        public FeatureArray Data { get; set; }

        public InputMessage(string source, FeatureArray data)
        {
            Data = data;
            Source = source;
        }
    }
}
