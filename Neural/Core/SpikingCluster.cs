using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    /// <summary>
    /// A localized network of nodes operating as a unit.    
    /// Analogous to a 'brain wrinkle'.
    /// </summary>
    public class SpikingCluster
    {
        public string Id { get; set; }
        public IEnumerable<SpikingNeuron> Neurons { get; set; }
        //public float CurrentCharge { get; set; } // Applied against Neuron Rest values
        public Dictionary<ulong, float> Modifications { get; set; } // Used in Threshold, Leak, and Rest calculations
    }
}
