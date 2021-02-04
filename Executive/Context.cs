
using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuikGraph;


namespace AIkailo.Executive
{
    internal class Context : ContextBase
    {
        private readonly IDataProvider _dataProvider;
        private readonly Queue<Node> _incoming = new Queue<Node>();
        private readonly AdjacencyGraph<Node, Connection> _nodes = new AdjacencyGraph<Node, Connection>();
        private readonly List<Context> _layer = new List<Context>();
        
        private int _current = 0;

        internal Context(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;            
        }

        internal override void Next()
        {
            // Add new Nodes            
            while (_incoming.Count > 0) // TODO: better handling for high volume. batch or throttle.
            {
                // TODO: Deduplicate
                _nodes.AddVertex(_incoming.Dequeue());
            }

            if (_nodes.VertexCount == 0) { return; }

            // Load the forward edges of the current layer            
            _nodes.AddEdgeRange(_dataProvider.GetEdges(_nodes.Vertices)); // TODO: Deduplicate, filter via weight thresholds?

            // Get the forward nodes with attention. Create layer clusters.

            // Get missing nodes of process models. 

            // Separate missing and ready nodes. Create layer clusters.

            // FeedForward ready nodes to calculate the new layers.            

            // Handle outbound nodes. Remove from layer.
            
            // Advance to the next layer
            _current++;

            /*** Considerations ***/

            // Grouping and 

            // Create groups
            // Find similar / matching scenes in DB. Apply tags.
            // Distribute to subContext if needed
            // look for / request / publish action for a scene representing the missing concepts
            // resolve and publish frames (in-process-out)


            /*
            * 
           
           - Resolve: match with a known scene with a process model and all the required concepts
           - decay
           - urgency, importance
           - noise filtering
           - Reduce local scene            
           */
        }

        internal Task Merge(Node node)
        {
            _incoming.Enqueue(node);
            //_current.AddVertex(node);            
            return Task.CompletedTask;
        }
    }
}