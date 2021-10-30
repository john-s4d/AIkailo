using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public interface INeuronProvider
    {           
        INeuron MergeInputNeuron(string source, Feature data);
        INeuron MergeOutputNeuron(string target, Feature data);
        INeuron CreateNeuronAssociation(IEnumerable<INeuron> input, IEnumerable<INeuron> output);        
        IEnumerable<ISynapse> MergeSynapsesBetween(INeuron startNode, IEnumerable<INeuron> finishNeurons);
        IEnumerable<ISynapse> MergeSynapsesBetween(IEnumerable<INeuron> startNeuron, INeuron finishNeuron);
        INeuron GetInputNeuron(string source, Feature data);
        INeuron GetOutputNeuron(string target, Feature data);
        IEnumerable<ISynapse> GetSynapsesBetween(INeuron startNode, IEnumerable<INeuron> finishNeurons);
        IEnumerable<ISynapse> GetSynapsesBetween(IEnumerable<INeuron> startNeurons, INeuron finishNeuron);
        IEnumerable<ISynapse> GetSynapsesBetween(IEnumerable<INeuron> startNeurons, IEnumerable<INeuron> finishNeurons);
        IEnumerable<ISynapse> GetSynapsesTo(IEnumerable<INeuron> finishNeurons);
        void FillForwardSynapses(IEnumerable<INeuron> currentLayer);
        List<INeuron> GetForwardNeurons(List<INeuron> incomingLayer);
        IEnumerable<INeuron> GetEmbeddedInputNeurons(INeuron Neuron);
        ulong NextId();
    }
}