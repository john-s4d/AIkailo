using AIkailo.Common;
using AIkailo.Neural.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public class Trainer
    {
        private INodeProvider _nodeProvider;

        public Trainer(INodeProvider nodeProvider)
        {
            _nodeProvider = nodeProvider;
        }

        public void MergeHint(List<Node> inputNodes, List<Node> outputNodes)
        {
            // Get/create a hint node with neutral weight connections.
            _ = _nodeProvider.MergeHintNode(inputNodes, outputNodes);
        }
    }
}
