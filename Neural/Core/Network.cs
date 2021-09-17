using AIkailo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    public class Network
    {
        private INodeProvider _nodeProvider;

        private Dictionary<string, Neuron> _neuronMap = new Dictionary<string, Neuron>();
        private Dictionary<string, Synapse> _synapseInMap = new Dictionary<string, Synapse>();
        private Dictionary<string, Synapse> _synapseOutMap = new Dictionary<string, Synapse>();

        private readonly object _inputLock = new object();

        public Network(INodeProvider nodeProvider)
        {
            _nodeProvider = nodeProvider;
        }

        private void Add(Neuron neuron)
        {

        }

        public void Input(IEnumerable<INeuron> neurons)
        {
            foreach (Neuron neuron in neurons)
            {
                if (neuron.Id == null) { throw new ArgumentNullException(nameof(neuron.Id)); }

                if (!_neuronMap.ContainsKey(neuron.Id))
                {
                    // Add the neuron to the network with the provided charge
                    _neuronMap.Add(neuron.Id, neuron);
                    foreach(Synapse synapse in neuron.SynapsesOut)
                    {
                        _synapseOutMap.Add(synapse.Id, synapse);
                    }
                }
                else
                {
                    // Increase the existing neuron's charge by the provided charge
                    _neuronMap[neuron.Id].Generate(neuron.CurrentCharge);
                }

            }
        }

        public void OnTick(TickEventArgs e)
        {
            foreach (Neuron neuron in _neuronMap.Values)
            {
                neuron.Tick();
            }
        }
    }
}
