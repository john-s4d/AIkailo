using AIkailo.Common;
using AIkailo.External.Common;
using AIkailo.Neural.Core;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    public class NeuronProvider : INeuronProvider
    {
        private GraphProvider _dataProvider;
        private long _nextId = 0; // TODO: Load from DB

        //private NeuronCache _NeuronCache = new NeuronCache();

        public NeuronProvider(GraphProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /*** Merge ***/

        public INeuron MergeInputNeuron(string source, Feature data)
        {
            var neuron = new SpikingNeuron();// { NodeType = NeuronType.INPUT };

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between -1 and +1, apply the value directly to the Neuron.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                neuron.Label = $"{source}:{label}"; // TODO: Sanitize,Escape
                neuron.Generate(value);
            }
            else
            {
                neuron.Label = $"{source}:{label}:{value}"; // TODO: Sanitize,Escape
            }

            IRecord record = _dataProvider.MergeNode(neuron.Label);
            neuron.Id = (ulong)record["n.id"];

            return neuron;
        }

        public INeuron MergeOutputNeuron(string target, Feature data)
        {
            var neuron = new SpikingNeuron();

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float betwewn [-1,1], apply the value directly to the Neuron.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                neuron.Label = $"{target}:{label}"; // TODO: Sanitize,Escape                
                neuron.Generate(value);
            }
            else
            {
                neuron.Label = $"{target}:{label}:{value}"; // TODO: Sanitize,Escape
            }

            IRecord record = _dataProvider.MergeNode(neuron.Label);
            neuron.Id = (ulong)record["n.id"];

            return neuron;
        }
        
        public INeuron CreateNeuronAssociation(IEnumerable<INeuron> input, IEnumerable<INeuron> output)
        {
            var Neuron = new SpikingNeuron();

            IRecord record = _dataProvider.MergeNodeBetween(
                input.Select(x => x.Id).ToArray(),
                output.Select(x => x.Id).ToArray()//,
                //NeuronType.HINT.ToString()
             );

            Neuron.Id = (ulong)record["n.id"];
            return Neuron;
        }

        public IEnumerable<ISynapse> MergeSynapsesBetween(INeuron startNeuron, IEnumerable<INeuron> finishNeurons)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> MergeSynapsesBetween(IEnumerable<INeuron> startNeurons, INeuron finishNeuron)
        {
            throw new NotImplementedException();
        }

        /*** Get ***/

        public INeuron GetInputNeuron(string source, Feature data)
        {
            throw new NotImplementedException();
        }

        public INeuron GetOutputNeuron(string source, Feature data)
        {
            throw new NotImplementedException();
        }
        /*
        public IEnumerable<INeuron> GetHintNeurons(IEnumerable<INeuron> input, IEnumerable<INeuron> output)
        {
            throw new NotImplementedException();
        }*/

        public void FillForwardSynapses(IEnumerable<INeuron> Neurons)
        {
            Dictionary<ulong, SpikingNeuron> NeuronsById = new Dictionary<ulong, SpikingNeuron>();

            foreach (SpikingNeuron n in Neurons)
            {
                NeuronsById.Add(n.Id, n);
            }

            foreach (IRecord record in _dataProvider.GetEdgesFrom(NeuronsById.Keys))
            {
                Synapse e = new Synapse();
                e.Id = (ulong)record["e.id"];
                e.Source = NeuronsById[(ulong)record["n1.id"]];
                e.Target = new SpikingNeuron() { Id = (ulong)record["n2.id"]};
                NeuronsById[e.Source.Id].SynapsesOut.Add(e);
            }
            //return result;
        }

        public IEnumerable<ISynapse> GetSynapsesTo(IEnumerable<INeuron> finishNeurons)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetSynapsesBetween(INeuron startNeuron, IEnumerable<INeuron> finishNeurons)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetSynapsesBetween(IEnumerable<INeuron> startNeurons, IEnumerable<INeuron> finishNeurons)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetSynapsesBetween(IEnumerable<INeuron> startNeurons, INeuron finishNeuron)
        {
            throw new NotImplementedException();
        }

        public List<INeuron> GetForwardNeurons(List<INeuron> incomingLayer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INeuron> GetEmbeddedInputNeurons(INeuron Neuron)
        {
            throw new NotImplementedException();
        }

        public ulong NextId()
        {
            return Convert.ToUInt64(Interlocked.Increment(ref _nextId));
        }
    }
}
