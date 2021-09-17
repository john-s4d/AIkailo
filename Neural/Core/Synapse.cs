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
    public class Synapse : ISynapse
    {
        public string Id { get; set; }
        public INeuron Source { get; set; }
        public INeuron Target { get; set; }

        public float MaxCharge { get; set; } // Max voltage this synapse can hold
        public float CurrentCharge { get; set; } // Current voltage        
    }
}