using AIkailo.External.Common;
using System.Collections.Generic;

namespace AIkailo.Common
{
    public interface INeuron
    {
        ulong Id { get; set; }
        Property Label { get; set; }
        //IEnumerable<ISynapse> SynapsesOut { get; }
    }
}