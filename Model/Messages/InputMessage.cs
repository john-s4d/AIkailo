using System;
using System.Collections.Generic;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Model
{
    public class InputMessage : IMessage
    {   
        public List<Tuple<IConvertible,IConvertible>> Data { get; }
        public string Sender { get; }

        public InputMessage(string sender,  List<Tuple<IConvertible, IConvertible>> data)
        {   
            Data = data;
            Sender = sender;
        }
    }
}
