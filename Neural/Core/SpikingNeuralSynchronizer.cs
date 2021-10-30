using AIkailo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    public class SpikingNeuralSynchronizer : INeuralSynchronizer
    {
        public event Action Charge;
        public event Action PreSpike;
        public event Action Spike;
        public event Action PostSpike;
        public event Action Leak;
        public event Action Adapt;

        public SpikingNeuralSynchronizer(ITimeProvider timeProvider)
        {
            timeProvider.Tick += OnTick;
        }

        public void OnTick(TickEventArgs e)
        {
            Charge?.Invoke();
            PreSpike?.Invoke();
            Spike?.Invoke();
            PostSpike?.Invoke();
            Leak?.Invoke();
            Adapt?.Invoke();           
        }
    }
}
