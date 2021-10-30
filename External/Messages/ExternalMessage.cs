using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Messaging.Messages
{
    public class ExternalMessage : IMessage
    {
        public string Target { get; set; }
        //public FeatureArray Data { get; set; }
        public Dictionary<ulong, float> Data { get; set; }

        //public ExternalMessage(string target, FeatureArray data)
        public ExternalMessage(string target, Dictionary<ulong, float> data)
        {
            Target = target;
            Data = data;
        }
    }
}
