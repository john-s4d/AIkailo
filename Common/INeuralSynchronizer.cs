using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Common
{
    public interface INeuralSynchronizer
    {
        void OnTick(TickEventArgs e);
    }
}
