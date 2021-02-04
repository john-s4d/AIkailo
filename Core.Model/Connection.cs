using AIkailo.External.Model;
using QuikGraph;

namespace AIkailo.Core.Model
{
    public class Connection : IEdge<Node>
    {
        public string Id { get; set; }
        public Node Source { get; set; }
        public Node Target { get; set; }
        public Property Label { get; set; }
        public float Value { get; set; }
        public FeatureArray Features { get; set; }
        
    }
}