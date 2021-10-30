using AIkailo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    public class SpikingNetworkAlgorithms
    {
        public static IEnumerable<SpikingNeuron> GetTargetNeuronIntersections(IEnumerable<SpikingNeuron> neurons)
        {

            return neurons
                    .Skip(1)
                    .Aggregate(
                        new HashSet<SpikingNeuron>(neurons.First().SynapsesOut.Select(n => n.Target)),
                        (h, e) => { h.IntersectWith(e.SynapsesOut.Select(n => n.Target)); return h; }
                    );
        }

        internal static IEnumerable<SpikingNeuron> GetSourceNeuronIntersections(IEnumerable<SpikingNeuron> neurons)
        {
            return neurons
                   .Skip(1)
                   .Aggregate(
                       new HashSet<SpikingNeuron>(neurons.First().SynapsesIn.Select(n => n.Source)),
                       (h, e) => { h.IntersectWith(e.SynapsesIn.Select(n => n.Source)); return h; }
                   );
        }
    }
}
