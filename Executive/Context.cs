
using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuikGraph;

namespace AIkailo.Executive
{
    internal class Context : ContextBase
    {
        private readonly Context _parent;
        private readonly IDataProvider _dataProvider;
        private readonly IExternalProvider _externalProvider;
        private readonly Queue<Node> _incoming = new Queue<Node>();
        
        private readonly List<AdjacencyGraph<Node, Connection>> _layer = new List<AdjacencyGraph<Node, Connection>>();                
        private int _current = 0;

        private readonly List<Context> _subContexts = new List<Context>();

        internal Context(IDataProvider dataProvider, IExternalProvider externalProvider, Context parent)
        {
            _dataProvider = dataProvider;
            _externalProvider = externalProvider;
            _parent = parent;
            _layer.Add(new AdjacencyGraph<Node, Connection>());
        }

        internal override void Next()
        {
            // Add new Nodes            
            while (_incoming.Count > 0) // TODO: better handling for high volume. batch or throttle.
            {
                // TODO: Deduplicate
                _layer[_current].AddVertex(_incoming.Dequeue());
            }

            if (_layer.Count == 0 || _layer[_current].VertexCount == 0) { return; }

            // Load the forward edges of the current layer            
            _layer[_current].AddEdgeRange(_dataProvider.GetEdges(_layer[_current].Vertices)); // TODO: Deduplicate, filter via weight thresholds?

            // Calculate attention. Get the forward nodes.

            // Get required subnet nodes for process models. Mark nodes as missing.

            // Create clusters and spawn new contexts for the smaller clusters.

            // - Mine previous layers for neighbors of the missing nodes. Bayes comparison. "most likely"
            // - Check if other contexts have activated neighbors, join contexts
            // - FeedBackwards to figure out what inputs are needed. 

            // FeedForward

            // Handle outbound nodes.

            // Advance to the next layer

            _layer.Add(new AdjacencyGraph<Node, Connection>());

            _current++;

            /*** Considerations ***/

            // Grouping and subcontexts            
            // resolve all connections (in-process-out)

            /*
           - decay
           - urgency, importance           
           - reward/emotion/sentiment
           */
        }

        internal Task Incoming(Node node)
        {
            _incoming.Enqueue(node);                   
            return Task.CompletedTask;
        }
    }
}