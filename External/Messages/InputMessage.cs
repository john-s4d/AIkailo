using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Messaging.Messages
{
    public class InputMessage : IMessage
    {
        public string Source { get; set; }
        public Dictionary<ulong, float> Data { get; set; }

        //public FeatureArray Data { get; set; }
        //public InputMessage(string source, FeatureArray data)
        public InputMessage(string source, Dictionary<ulong, float> data)
        {
            Data = data;
            Source = source;
        }
    }
}
