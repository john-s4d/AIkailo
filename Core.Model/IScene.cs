using System.Collections.Generic;
using QuikGraph;

namespace AIkailo.Core.Model
{
    public interface IScene : IMutableVertexAndEdgeListGraph<IConcept, IConceptEdge>
    { 
        IConcept Target { get; set; }
    }

}
