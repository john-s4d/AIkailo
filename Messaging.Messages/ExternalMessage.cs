using AIkailo.External.Model;

namespace AIkailo.Messaging.Messages
{
    public class ExternalMessage : IMessage
    {   
        public string Target { get; set; }
        public FeatureArray Data { get; set; }

        public ExternalMessage(string target, FeatureArray data) 
        {
            Target = target;
            Data = data;
        }
    }
}
