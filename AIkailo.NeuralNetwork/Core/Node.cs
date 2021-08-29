using AIkailo.External.Common;
using System;
using System.Collections.Generic;

namespace AIkailo.Neural.Core
{
    public class Node
    {
        public NodeType NodeType { get; set; } = NodeType.EMPTY;
        public string Id { get; set; }
        public Property Label { get; set; }
        public float Value { get; set; }
        public float Bias { get; set; }
        public List<Edge> ForwardEdges { get; private set; }
        public FeatureArray Features { get; set; }

        public virtual void FeedForward()
        {

        }

        public void AddForwardEdge(Edge e)
        {
            if (ForwardEdges == null)
            {
                ForwardEdges = new List<Edge>();
            }
            ForwardEdges.Add(e);
        }
    }

    public enum NodeType
    {
        EMPTY,        
        INPUT,
        HIDDEN,
        OUTPUT,
        EMBEDDED,
        TRANSFORM,
        HINT
    }

    public enum NodeStatus
    {
        INACTIVE,
        ACTIVE,
        MISSING
    }
}