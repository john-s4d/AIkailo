using System;
using System.Collections.Generic;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Model
{
    public class ExternalMessage : IMessage
    {
        public List<Tuple<IConvertible, IConvertible>> Data { get; }

        public ExternalMessage(List<Tuple<IConvertible, IConvertible>> data) 
        {
            Data = data;
        }
    }
}
