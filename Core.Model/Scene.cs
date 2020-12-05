using QuikGraph;
using System;

namespace AIkailo.Core.Model
{
    public class Scene : AdjacencyGraph<Concept, ConceptEdge> //, IMutableVertexAndEdgeListGraph<Concept, ConceptEdge>
    {
        public IConvertible Target { get; set; }
        public string Id { get; set; }

        public Scene() :
            base()
        { }

        public Scene(Concept target, string id = null)
            : base()
        {
            Target = target;
            Id = id;
        }
    }
}