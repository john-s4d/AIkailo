using QuikGraph;
using System;

namespace AIkailo.Core.Model
{
    public class Scene : AdjacencyGraph<Concept, ConceptEdge> //, IMutableVertexAndEdgeListGraph<Concept, ConceptEdge>
    {
        public Concept Target { get; set; }
        public string Id { 
            get { return Target.Id; }

        }

        public Scene() :
            base()
        { }

        public Scene(Concept target = null)
            : base()
        {
            Target = target;
        }
    }
}