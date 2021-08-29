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

        public Node MergeInputNode(string source, Feature data)
        {
            var node = new Node() { NodeType = NodeType.INPUT };

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between -1 and +1, apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                node.Label = $"{source}:{label}"; // TODO: Sanitize,Escape
                node.Value = value;
            }
            else
            {
                node.Label = $"{source}:{label}:{value}"; // TODO: Sanitize,Escape
                node.Value = 1;
            }

            IRecord record = _dataProvider.MergeNode(node.NodeType.ToString(), node.Label);
            node.Id = record["n.id"].ToString();

            return node;
        }

        public Node MergeOutputNode(string target, Feature data)
        {
            var node = new Node() { NodeType = NodeType.OUTPUT };

            var label = data.Item1;
            var value = data.Item2;

            // If this is a float between 0-1, apply the value directly to the node.

            if (value.TypeCode == TypeCode.Single && value >= -1 && value <= 1)
            {
                node.Label = $"{target}:{label}"; // TODO: Sanitize,Escape
                node.Value = value;
            }
            else
            {
                node.Label = $"{target}:{label}:{value}"; // TODO: Sanitize,Escape
                node.Value = 1;
            }

            IRecord record = _dataProvider.MergeNode(node.NodeType.ToString(), node.Label);
            node.Id = record["n.id"].ToString();

            return node;
        }

        public Node MergeHintNode(IEnumerable<Node> input, IEnumerable<Node> output)
        {
            var node = new Node() { NodeType = NodeType.HINT };

            IRecord record = _dataProvider.MergeNodeBetween(
                input.Select(x => x.Id).ToArray(),
                output.Select(x => x.Id).ToArray(),
                NodeType.HINT.ToString()
             );

            node.Id = record["n.id"].ToString();
            return node;
        }

        public IEnumerable<Edge> MergeEdgesBetween(Node startNode, IEnumerable<Node> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Edge> MergeEdgesBetween(IEnumerable<Node> startNodes, Node finishNode)
        {
            throw new NotImplementedException();
        }

        /*** Get ***/

        public Node GetInputNode(string source, Feature data)
        {
            throw new NotImplementedException();
        }

        public Node GetOutputNode(string source, Feature data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetHintNodes(IEnumerable<Node> input, IEnumerable<Node> output)
        {
            throw new NotImplementedException();
        }

        public void FillForwardEdges(IEnumerable<Node> nodes)
        {
            Dictionary<string, Node> nodesById = new Dictionary<string, Node>();

            foreach (Node n in nodes)
            {
                nodesById.Add(n.Id, n);
            }

            foreach (IRecord record in _dataProvider.GetEdgesFrom(nodesById.Keys))
            {
                Edge e = new Edge();
                e.Id = (string)record["e.id"];
                e.Source = nodesById[(string)record["n1.id"]];
                e.Target = new Node() { Id = (string)record["n2.id"], NodeType = (NodeType)Enum.Parse(typeof(NodeType), (string)record["n2.type"]) };
                nodesById[e.Source.Id].AddForwardEdge(e);
            }
            //return result;
        }

        public IEnumerable<Edge> GetEdgesTo(IEnumerable<Node> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Edge> GetEdgesBetween(Node startNode, IEnumerable<Node> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Edge> GetEdgesBetween(IEnumerable<Node> startNodes, IEnumerable<Node> finishNodes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Edge> GetEdgesBetween(IEnumerable<Node> startNodes, Node finishNode)
        {
            throw new NotImplementedException();
        }

        public List<Node> GetForwardNodes(List<Node> incomingLayer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetEmbeddedInputNodes(Node node)
        {
            throw new NotImplementedException();
        }
    }
}
