
using AIkailo.Core.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuikGraph;

namespace AIkailo.Executive
{
    public class Context : ContextBase
    {
        private readonly Context _parent;
        private readonly INodeProvider _nodeFactory;
        private readonly IExternalProvider _externalProvider;

        private readonly Queue<Node> _incoming = new Queue<Node>();
        //private readonly List<AdjacencyGraph<Node, Edge>> _layers = new List<AdjacencyGraph<Node, Edge>>();
        private readonly List<Node> _currentLayer;
        private readonly List<Node> _forwardLayer;

        private int _current = 0;

        private readonly List<Context> _subContexts = new List<Context>();

        internal Context(INodeProvider nodeFactory, IExternalProvider externalProvider, Context parent)
        {
            _nodeFactory = nodeFactory;
            _externalProvider = externalProvider;
            _parent = parent;
            _currentLayer = new List<Node>();

        }

        internal override void Next()
        {
            // Add new Nodes            
            while (_incoming.Count > 0) // TODO: better handling for high volume. batch or throttle?
            {         
                _currentLayer.Add(_incoming.Dequeue());
            }

            if (_currentLayer.Count == 0) { return; }
                        
            
            // Load the forward edges and nodes of the current layer
            // TODO: Deduplicate, filter via weight thresholds?  Can we omit the forward nodes?
            _nodeFactory.FillForwardEdges(_currentLayer);

            // TODO: Calculate attention. Drop un-needed edges and nodes.

            

            foreach (Node node in _currentLayer)
            {
                foreach (Edge e in node.ForwardEdges) {
                    _forwardLayer.Add(e.Target);
                }
            }

            // Evaluate models


            // Get required subnet nodes for process models. Mark nodes as missing.

            // Create clusters and spawn new contexts for the smaller clusters.

            // - Mine previous layers for neighbors of the missing nodes. Bayes comparison. "most likely"
            // - Check if other contexts have activated neighbors, join contexts
            // - FeedBackwards to figure out what inputs are needed. 

            // FeedForward

            // Carry forward or split unused nodes

            // Handle outbound nodes.

            // Advance to the next layer

            //_layers.Add(new AdjacencyGraph<Node, Edge>());

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

        internal void Incoming(Node node)
        {
            _incoming.Enqueue(node);
        }
    }
}