using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public interface INeuron
    {
        ulong Id { get; }
        Property Label { get; }
        NeuronType Type { get; }

        //IEnumerable<ISynapse> SynapsesOut { get; }
    }
}