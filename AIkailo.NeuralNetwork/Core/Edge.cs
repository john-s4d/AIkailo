//using AIkailo.External.Common;
//using QuikGraph;

namespace AIkailo.Core.Common
{
    public class Edge //: IEdge<Node>
    {
        public string Id { get; set; }
        public Node Source { get; set; }
        public Node Target { get; set; }
        public float Weight { get; set; }
        
    }
}