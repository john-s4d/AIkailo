using AIkailo.External.Common;
using QuikGraph;

namespace AIkailo.Core.Common
{
    public class Edge : IEdge<Node>
    {
        public string Id { get; set; }
        public Node Source { get; set; }
        public Node Target { get; set; }
        public Property Label { get; set; }
        public float Value { get; set; }
        public FeatureArray Features { get; set; }
        
    }
}