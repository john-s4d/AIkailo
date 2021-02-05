
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
        }

        internal override void Next()
        {
            // Add new Nodes            
            while (_incoming.Count > 0) // TODO: better handling for high volume. batch or throttle.
            {
                // TODO: Deduplicate
                _layer[_current].AddVertex(_incoming.Dequeue());
            }

            if (_layer[_current].VertexCount == 0) { return; }

            // Load the forward edges of the current layer            
            _layer[_current].AddEdgeRange(_dataProvider.GetEdges(_layer[_current].Vertices)); // TODO: Deduplicate, filter via weight thresholds?

            // Get the forward nodes with attention. Create clusters. Spawn new contexts, largest cluster one stays in this one.

            // Get required subnet nodes for process models. 

            // Start new contexts to find suitable values for missing nodes.
            // - Mine previous layers for neighbors of the missing nodes. Bayes comparison. "most likely"
            // - Check if other contexts have activated neighbors, join contexts
            // - FeedBackwards to figure out what inputs are needed. 

            // FeedForward each cluster into new independent contexts

            // Handle outbound nodes.

            // Advance to the next layer

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

        internal Task Merge(Node node)
        {
            _incoming.Enqueue(node);
            //_current.AddVertex(node);            
            return Task.CompletedTask;
        }
    }
}