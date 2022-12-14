using AIkailo.Common;
using AIkailo.External.Common;
using AIkailo.Neural.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public class Agent
    {

        public Action<Dictionary<string, float>> Output;

        // Eventually we will have multiple networks working together.
        private readonly SpikingNeuralNetwork _network;        

        internal Agent(INeuronProvider neuronProvider, ITimeProvider timeProvider)
        {
            _network = new SpikingNeuralNetwork(neuronProvider, timeProvider);
            _network.Output += _network_Output;
        }

        private void _network_Output(Dictionary<string, float> output)
        {
            Output?.Invoke(output);
        }

        public void Input(Dictionary<string, float> data)
        {
            _network.Input(data);
        }

        public void Train(Dictionary<string,float> input, Dictionary<string,float> output)
        {

            // Ensure neurons exist
            _network.LoadNeurons(NeuronType.INPUT, input.Keys);
            _network.LoadNeurons(NeuronType.OUTPUT, output.Keys);

            // Connect input <=> output with a single neuron in between and set initial weights of the neuron & synapses
            _network.CreateAndTrainAssociations(input, output);

            // Normalize all the neurons and synapses between these two sets
            _network.Normalize(input.Keys, output.Keys);
            
        }
    }
}