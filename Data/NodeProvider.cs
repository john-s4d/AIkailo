using AIkailo.Common;
using AIkailo.External.Common;
using AIkailo.Neural.Core;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    public class NodeProvider : INodeProvider
    {
        private GraphProvider _dataProvider;
        //private NodeCache _nodeCache = new NodeCache();

        public NodeProvider(GraphProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /*** Merge ***/

        public INeuron MergeInputNode(string source, Feature data)
        {
            var neuron = new Neuron() { NodeType = NeuronType.INPUT };

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between -1 and +1, apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                neuron.Label = $"{source}:{label}"; // TODO: Sanitize,Escape
                //node.Bias = value;
            }
            else
            {
                neuron.Label = $"{source}:{label}:{value}"; // TODO: Sanitize,Escape
                //node.Bias = 1;
            }

            IRecord record = _dataProvider.MergeNode(neuron.NodeType.ToString(), neuron.Label);
            neuron.Id = record["n.id"].ToString();

            return neuron;
        }

        public INeuron MergeOutputNode(string target, Feature data)
        {
            var neuron = new Neuron() { NodeType = NeuronType.OUTPUT };

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float betwewn [-1,1], apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                neuron.Label = $"{target}:{label}"; // TODO: Sanitize,Escape
                //node.Bias = value;
            }
            else
            {
                neuron.Label = $"{target}:{label}:{value}"; // TODO: Sanitize,Escape
                //node.Bias = 1;
            }

            IRecord record = _dataProvider.MergeNode(neuron.NodeType.ToString(), neuron.Label);
            neuron.Id = record["n.id"].ToString();

            return neuron;
        }

        public INeuron MergeHintNode(IEnumerable<INeuron> input, IEnumerable<INeuron> output)
        {
            var node = new Neuron() { NodeType = NeuronType.HINT };

            IRecord record = _dataProvider.MergeNodeBetween(
                input.Select(x => x.Id).ToArray(),
                output.Select(x => x.Id).ToArray(),
                NeuronType.HINT.ToString()
             );

            node.Id = record["n.id"].ToString();
            return node;
        }

        public IEnumerable<ISynapse> MergeEdgesBetween(INeuron startNode, IEnumerable<INeuron> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> MergeEdgesBetween(IEnumerable<INeuron> startNodes, INeuron finishNode)
        {
            throw new NotImplementedException();
        }

        /*** Get ***/

        public INeuron GetInputNode(string source, Feature data)
        {
            throw new NotImplementedException();
        }

        public INeuron GetOutputNode(string source, Feature data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INeuron> GetHintNodes(IEnumerable<INeuron> input, IEnumerable<INeuron> output)
        {
            throw new NotImplementedException();
        }

        public void FillForwardEdges(IEnumerable<INeuron> nodes)
        {
            Dictionary<string, Neuron> nodesById = new Dictionary<string, Neuron>();

            foreach (Neuron n in nodes)
            {
                nodesById.Add(n.Id, n);
            }

            foreach (IRecord record in _dataProvider.GetEdgesFrom(nodesById.Keys))
            {
                Synapse e = new Synapse();
                e.Id = record["e.id"].ToString();
                e.Source = nodesById[record["n1.id"].ToString()];
                e.Target = new Neuron() { Id = record["n2.id"].ToString(), NodeType = (NeuronType)Enum.Parse(typeof(NeuronType), (string)record["n2.type"]) };
                nodesById[e.Source.Id].SynapsesOut.Add(e);
            }
            //return result;
        }

        public IEnumerable<ISynapse> GetEdgesTo(IEnumerable<INeuron> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetEdgesBetween(INeuron startNode, IEnumerable<INeuron> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetEdgesBetween(IEnumerable<INeuron> startNodes, IEnumerable<INeuron> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynapse> GetEdgesBetween(IEnumerable<INeuron> startNodes, INeuron finishNode)
        {
            throw new NotImplementedException();
        }

        public List<INeuron> GetForwardNodes(List<INeuron> incomingLayer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INeuron> GetEmbeddedInputNodes(INeuron node)
        {
            throw new NotImplementedException();
        }
    }
}
