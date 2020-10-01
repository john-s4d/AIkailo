using AIkailo.Common;

namespace AIkailo.Model.Internal
{
    public class InputMessage : IMessage
    {   

        public DataPackage Data { get; }
        public string Source { get; }

        public InputMessage(string source, DataPackage data)
        {
            Data = data;
            Source = source;
        }
    }
}
