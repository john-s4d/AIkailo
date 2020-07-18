using AIkailo.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Modules.Interaction
{
    public class InteractionSensorMessage : ISensorMessage
    {
        public string Data { get; set; }
        //public List<Tuple<IConvertible, IConvertible>> Data { get; set; } = new List<Tuple<IConvertible, IConvertible>>();
    }
}
