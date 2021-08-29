using AIkailo.External.Common;
using AIkailo.Neural.Core;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public interface INodeProvider
    {           
        Node MergeInputNode(string source, Feature data);
        Node MergeOutputNode(string target, Feature data);
        Node MergeHintNode(IEnumerable<Node> input, IEnumerable<Node> output);        
        IEnumerable<Edge> MergeEdgesBetween(Node startNode, IEnumerable<Node> finishNodes);
        IEnumerable<Edge> MergeEdgesBetween(IEnumerable<Node> startNodes, Node finishNode);
        Node GetInputNode(string source, Feature data);
        Node GetOutputNode(string target, Feature data);
        IEnumerable<Node> GetHintNodes(IEnumerable<Node> input, IEnumerable<Node> output);

        //IEnumerable<Edge> GetEdgesFrom(IEnumerable<Node> startNodes);
        IEnumerable<Edge> GetEdgesBetween(Node startNode, IEnumerable<Node> finishNodes);
        IEnumerable<Edge> GetEdgesBetween(IEnumerable<Node> startNodes, Node finishNode);
        IEnumerable<Edge> GetEdgesBetween(IEnumerable<Node> startNodes, IEnumerable<Node> finishNodes);
        IEnumerable<Edge> GetEdgesTo(IEnumerable<Node> finishNodes);
        void FillForwardEdges(IEnumerable<Node> currentLayer);
        List<Node> GetForwardNodes(List<Node> incomingLayer);
        IEnumerable<Node> GetEmbeddedInputNodes(Node node);
    }
}