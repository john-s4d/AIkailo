using System;
using System.Collections;
using System.Collections.Generic;

namespace AIkailo.Data.DBCore
{
    public class TreeTraverser<K, V> : IEnumerable<Tuple<K, V>>
    {
        private readonly TreeNode<K, V> _fromNode;
        private readonly int _fromIndex;
        private readonly TreeTraverseDirection _direction;
        private readonly ITreeNodeManager<K, V> _nodeManager;

        public TreeTraverser(ITreeNodeManager<K, V> nodeManager, TreeNode<K, V> fromNode,
            int fromIndex, TreeTraverseDirection direction)
        {
            _direction = direction;
            _fromIndex = fromIndex;
            _fromNode = fromNode ?? throw new ArgumentNullException(nameof(fromNode));
            _nodeManager = nodeManager;
        }

        IEnumerator<Tuple<K, V>> IEnumerable<Tuple<K, V>>.GetEnumerator()
        {
            return new TreeEnumerator<K, V>(_nodeManager, _fromNode, _fromIndex, _direction);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Use the generic version
            return ((IEnumerable<Tuple<K, V>>)this).GetEnumerator();
        }
    }
}