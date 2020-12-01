using QuikGraph;

namespace AIkailo.Core.Model
{
    public class Scene : AdjacencyGraph<Concept, ConceptEdge>, IMutableVertexAndEdgeListGraph<Concept, ConceptEdge>
    {   
        public AdjacencyGraph<Concept, ConceptEdge> Graph { get; set; }
        public Concept Target { get; set; }
    }
}