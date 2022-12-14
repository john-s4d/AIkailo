using AIkailo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Neural.Core
{
    public class SpikingNeuralNetwork : INeuralNetwork
    {
        private INeuronProvider _neuronProvider;        

        private SpikingNeuralSynchronizer _synchronizer;

        private Dictionary<ulong, SpikingNeuron> _neuronsById = new Dictionary<ulong, SpikingNeuron>();
        private Dictionary<string, SpikingNeuron> _neuronsByLabel = new Dictionary<string, SpikingNeuron>();

        private Queue<SpikingNeuron> _spikedOutputNeurons = new Queue<SpikingNeuron>();
        private Queue<Tuple<ulong,float>> _incomingCharges = new Queue<Tuple<ulong, float>>();

        public event Action<Dictionary<string, float>> Output;
        public event Action<Dictionary<ulong, float>> NeuronActivity;
        public event Action<Dictionary<ulong, float>> ClusterActivity;

        public SpikingNeuralNetwork(INeuronProvider neuronProvider, ITimeProvider timeProvider)
        {
            _synchronizer = new SpikingNeuralSynchronizer(timeProvider);
            _neuronProvider = neuronProvider;
            
            _synchronizer.Charge += Charge;
            _synchronizer.PostSpike += PostSpike;
            
        }

        private void Charge()
        {
            while(_incomingCharges.Count > 0)
            {
                Tuple<ulong, float> charge = _incomingCharges.Dequeue();
                _neuronsById[charge.Item1].Charge(charge.Item2);
            }
        }

        private void PostSpike()
        {
            Dictionary<string, float> output = new Dictionary<string, float>();
            
            while(_spikedOutputNeurons.Count > 0)
            {
                SpikingNeuron neuron = _spikedOutputNeurons.Dequeue();
                output.Add(neuron.Label, neuron.Potential);
            }
            Output?.Invoke(output);            
        }

        public float AverageThreshold()
        {
            return _neuronsById.Values.Average(n => n.Threshold);
        }

        public float AverageRest()
        {
            return _neuronsById.Values.Average(n => n.Rest);
        }

        public float AverageLeak()
        {
            return _neuronsById.Values.Average(n => n.Leak);
        }

        public void Input(Dictionary<string, float> data)
        {
            foreach(KeyValuePair<string, float> dataPoint in data)
            {
                _incomingCharges.Enqueue(new Tuple<ulong,float>(_neuronsByLabel[dataPoint.Key].Id, dataPoint.Value));
            }
        }

        public void ClusterModification(Dictionary<ulong, Tuple<ulong, float>> data)
        {
            throw new NotImplementedException();
        }

        public void LoadNeurons(NeuronType type, IEnumerable<string> labels)
        {
            foreach (string label in labels)
            {
                if (!_neuronsByLabel.ContainsKey(label))
                {
                    //SpikingNeuron neuron = _neuronProvider.MergeNeuron(label);
                    SpikingNeuron neuron = new SpikingNeuron(type, label, _neuronProvider.NextId(), _synchronizer);

                    // Set initial values.
                    if (type == NeuronType.HIDDEN)
                    {
                        if (_neuronsById.Count > 0)
                        {
                            neuron.Threshold = AverageThreshold();
                            neuron.Rest = AverageRest();
                            neuron.Leak = AverageLeak();
                        }
                    }

                    _neuronsByLabel.Add(neuron.Label, neuron);
                    _neuronsById.Add(neuron.Id, neuron);
                    neuron.Spike += Neuron_Spike;
                }
            }
        }

        private void Neuron_Spike(SpikingNeuron neuron)
        {
            if (neuron.Type == NeuronType.OUTPUT)
            {
                _spikedOutputNeurons.Enqueue(neuron);
            }
            
        }

        /// <summary>
        /// Connect input neurons to output neurons via a single balanced neuron
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void CreateAndTrainAssociations(Dictionary<string, float> input, Dictionary<string, float> output)
        {            
            
            // It's expected that all the provided neurons already exist in the network
            var inputNeurons = GetNeurons(input.Keys);
            var outputNeurons = GetNeurons(output.Keys);

            var intersectingInputTargets = SpikingNetworkAlgorithms.GetTargetNeuronIntersections(inputNeurons);
            var intersectingOutputSources = SpikingNetworkAlgorithms.GetSourceNeuronIntersections(outputNeurons);
            var intersectingNeurons = intersectingInputTargets.Intersect(intersectingOutputSources);

            // TODO: Should it be an exclusive association?

            SpikingNeuron newNeuron = null;

            if (intersectingNeurons.Count() == 0)
            {
                newNeuron = new SpikingNeuron(NeuronType.HIDDEN, _neuronProvider.NextId(), _synchronizer);

                foreach(SpikingNeuron inputNeuron in inputNeurons)
                {
                    SpikingNeuron.CreateSynapse(inputNeuron, newNeuron);                    
                    
                }
                foreach(SpikingNeuron outputNeuron in outputNeurons)
                {
                    SpikingNeuron.CreateSynapse(newNeuron, outputNeuron);                    
                }
            }

            if (intersectingNeurons.Count() > 1)
            {
                // There's more than one association
                // TODO: Not sure how to handle this. Needs to be normalized.
                throw new NotImplementedException();
            }

            _neuronsById.Add(newNeuron.Id, newNeuron);

            Train(input, output, newNeuron);
        }

        public void Train(Dictionary<string, float> input, Dictionary<string, float> output, SpikingNeuron neuron)
        {
            // Set weights of the neuron & synapses so the input will generate the outpumt.

            foreach (string label in output.Keys)
            {
                //_neuronsByLabel[label].Threshold = output[label];
            }


            foreach (string label in input.Keys)
            {
                //_neuronsByLabel[label].Threshold = input[label];
            }

            
            throw new NotImplementedException();
        }

        private IEnumerable<SpikingNeuron> GetNeurons(IEnumerable<string> labels)
        {
            foreach(string label in labels)
            {
                yield return _neuronsByLabel[label];
            }
        }

        private IEnumerable<SpikingNeuron> GetNeurons(IEnumerable<ulong> ids)
        {
            foreach (ulong id in ids)
            {
                yield return _neuronsById[id];
            }
        }

        public void Normalize(IEnumerable<string> inputLabels, IEnumerable<string> outputLabels)
        {
            // Join and prune all the neurons between the input and output
            throw new NotImplementedException();
        }

        /*
        public void Input(IEnumerable<INeuron> neurons)
        {
            foreach (SpikingNeuron neuron in neurons)
            {
                if (neuron.Id == ulong.MinValue) { throw new ArgumentException(nameof(neuron.Id)); }

                if (!_neuronsById.ContainsKey(neuron.Id))
                {
                    // Add the neuron to the network with the provided charge
                    _neuronsById.Add(neuron.Id, neuron);
                    neuron.Synchronizer = _synchronizer;

                    foreach(Synapse synapse in neuron.SynapsesOut)
                    {
                        if (!_synapseOutMap.ContainsKey(synapse.Id))
                        {
                            _synapseOutMap.Add(synapse.Id, synapse);
                        }                        
                    }
                    
                    //foreach(Synapse synapse in neuron.SynapsesIn)
                    //{
                     //   if (!_synapseInMap.ContainsKey(synapse.Id))
                      //  {
                      //      _synapseInMap.Add(synapse.Id, synapse);
                       // }                        
                   // }
                   //
                }
                else
                {
                    // Increase the existing neuron's charge by the provided charge
                    _neuronsById[neuron.Id].Generate(neuron.Potential);
                }
            }
        }*/


    }
}
