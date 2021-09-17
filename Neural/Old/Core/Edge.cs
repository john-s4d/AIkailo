//using AIkailo.External.Common;
//using QuikGraph;

using System;

namespace AIkailo.Neural.Old.Core
{
    /// <summary>
    /// A fixed connection between two nodes.
    /// Analogous to a Synapse.
    /// </summary>
    public class Edge //: IEdge<Node>
    {
        public string Id { get; set; }
        public Node Source { get; set; }
        public Node Target { get; set; }
        public float Weight { get; set; }
        public float Speed { get; set; }
        public EdgeDirection Direction { get; set; }
    }

    public enum EdgeDirection
    {
        FORWARD,
        BACKWARD
    }
}