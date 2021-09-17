using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    /// <summary>
    /// A localized network of nodes operating as a unit.
    /// Node edges may connect to nodes in other clusters.
    /// Analogous to a 'brain wrinkle'.
    /// </summary>
    public class Cluster
    {
        public string Id { get; set; }
        public List<Neuron> Neurons { get; set; }
        //public float CurrentCharge { get; set; }
    }
}
