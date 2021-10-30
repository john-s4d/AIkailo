//using AIkailo.External.Common;
//using QuikGraph;

using System;
using System.Threading.Tasks;
using AIkailo.Common;

namespace AIkailo.Neural.Core
{
    /// <summary>
    /// A fixed connection between two nodes.
    /// Analogous to a Synapse.
    /// </summary>
    public class SpikingSynapse : ISynapse
    {
        public ulong Id { get; set; }
        public SpikingNeuron Source { get; set; }
        public SpikingNeuron Target { get; set; }

        // Weight must be between [0,1]
        private float _weight = 0;
        public float Weight { 
            get { return _weight; }
            set { _weight = value >= 0 || value <= 1 ? value : throw new ArgumentOutOfRangeException(nameof(Weight)); }
        } 
        public float Potential { get; set; } // Current voltage        

        //public float MaxPotential { get; set; } // Max voltage this synapse can hold
    }
}