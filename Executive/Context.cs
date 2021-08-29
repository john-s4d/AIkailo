using AIkailo.Common;
using AIkailo.Neural.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public class Context : ContextBase
    {
        private Context _parent;
        private readonly INodeProvider _nodeProvider;
        private readonly IExternalProvider _externalProvider;

        private Queue<Node> _incoming = new Queue<Node>();

        private List<Node> _incomingLayer = new List<Node>();
        private List<Node> _forwardLayer;

        private int _current = 0;

        //private readonly List<Context> _subContexts = new List<Context>();

        internal Context(INodeProvider nodeProvider, IExternalProvider externalProvider, Context parent = null)
        {
            _nodeProvider = nodeProvider;
            _externalProvider = externalProvider;
            _parent = parent;

            _incomingLayer = new List<Node>();
        }

        internal override void Next()
        {
            // Add new Nodes            
            while (_incoming.Count > 0) // TODO: better handling for high volume. batch or throttle?
            {         
                _incomingLayer.Add(_incoming.Dequeue());
            }

            if (_incomingLayer.Count == 0) { return; }

            // Load the forward edges
            _nodeProvider.FillForwardEdges(_incomingLayer); // TODO: Deduplicate, filter via weight thresholds

            // TODO: Calculate attention. Drop un-needed edges and nodes.

            // Load the forward nodes
            _forwardLayer = _nodeProvider.GetForwardNodes(_incomingLayer);

            // Normalize network model

            // TODO: Move to private method so we can run it in a loop to get any new embedded nodes
            List<Node> embeddedIncoming = new List<Node>();
            foreach (Node node in _forwardLayer)
            {
                if (node.NodeType == NodeType.EMBEDDED)
                {
                    // TODO: Should this be immediately pushed into a new context?
                    embeddedIncoming.AddRange(_nodeProvider.GetEmbeddedInputNodes(node));
                }
            }
            // TODO: Calculate attention for the new nodes. 
            _forwardLayer.AddRange(_nodeProvider.GetForwardNodes(embeddedIncoming));

            // TODO: Drop un-needed edges and nodes.

            // Separate out the known networks.

            // Create clusters and spawn new contexts for the smaller clusters.

            // - Mine previous layers for neighbors of the missing nodes. Bayes comparison. "most likely"
            // - Check if other contexts have activated neighbors, join contexts
            // - FeedBackwards to figure out what inputs are needed. 

            // FeedForward

            // Carry forward or split unused nodes

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

        internal void Incoming(IEnumerable<Node> nodes)
        {
            // TODO: flow gates to ensure we don't start processing th nodes in the middle of an incoming batch
            foreach(Node node in nodes)
            {
                _incoming.Enqueue(node);
            }
        }
    }
}