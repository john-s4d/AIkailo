using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public interface INodeProvider
    {           
        INeuron MergeInputNode(string source, Feature data);
        INeuron MergeOutputNode(string target, Feature data);
        INeuron MergeHintNode(IEnumerable<INeuron> input, IEnumerable<INeuron> output);        
        IEnumerable<ISynapse> MergeEdgesBetween(INeuron startNode, IEnumerable<INeuron> finishNodes);
        IEnumerable<ISynapse> MergeEdgesBetween(IEnumerable<INeuron> startNodes, INeuron finishNode);
        INeuron GetInputNode(string source, Feature data);
        INeuron GetOutputNode(string target, Feature data);
        IEnumerable<INeuron> GetHintNodes(IEnumerable<INeuron> input, IEnumerable<INeuron> output);

        //IEnumerable<Edge> GetEdgesFrom(IEnumerable<Node> startNodes);
        IEnumerable<ISynapse> GetEdgesBetween(INeuron startNode, IEnumerable<INeuron> finishNodes);
        IEnumerable<ISynapse> GetEdgesBetween(IEnumerable<INeuron> startNodes, INeuron finishNode);
        IEnumerable<ISynapse> GetEdgesBetween(IEnumerable<INeuron> startNodes, IEnumerable<INeuron> finishNodes);
        IEnumerable<ISynapse> GetEdgesTo(IEnumerable<INeuron> finishNodes);
        void FillForwardEdges(IEnumerable<INeuron> currentLayer);
        List<INeuron> GetForwardNodes(List<INeuron> incomingLayer);
        IEnumerable<INeuron> GetEmbeddedInputNodes(INeuron node);
    }
}