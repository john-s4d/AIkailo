using AIkailo.Common;

namespace AIkailo.Model.Internal
{
    public class ExternalMessage : IMessage
    {
        public DataPackage Data { get; }

        public ExternalMessage(DataPackage data) 
        {
            Data = data;
        }
    }
}
