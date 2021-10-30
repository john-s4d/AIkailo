using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.External.Common
{
    public class TrainingData
    {
        public string Source;

        public string Target;

        public Dictionary<ulong, float> Input;

        public Dictionary<ulong, float> Output;
        
    }
}
