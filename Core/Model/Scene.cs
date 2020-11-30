using QuikGraph;

namespace AIkailo.Core.Model
{
    public class Scene : AdjacencyGraph<IConcept, IConceptEdge>,  IScene
    {   
        public AdjacencyGraph<IConcept, IConceptEdge> Graph { get; set; }
        public IConcept Target { get; set; }
    }
}