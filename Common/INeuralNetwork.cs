using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Common
{
    public interface INeuralNetwork
    {
        event Action<Dictionary<string, float>> Output;
        void Input(Dictionary<string, float> data);
       
    }
}
