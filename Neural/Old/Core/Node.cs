using AIkailo.External.Common;
using System;
using System.Collections.Generic;

namespace AIkailo.Neural.Old.Core
{
    /// <summary>
    /// A defined point in the network. 
    /// Can be grounded (with a label) or ungrounded (no label).
    /// May define a processing model
    /// Analogous to a Neuron.
    /// </summary>
    public class Node
    {
        public string Id { get; set; }
        public Cluster Cluster { get; set; }
        public NodeType NodeType { get; set; } = NodeType.UNKNOWN;
        public List<Edge> Edges { get; } = new List<Edge>();
        public Property Label { get; set; }
        public float Value { get; set; }
        public float Bias { get; set; } 
        public NodeStatus Status { get; set; }        
        public FeatureArray Features { get; set; } // Node features define how activation affects self and system. ie: reward, decay, etc..
    }

    public enum NodeType
    {
        UNKNOWN,
        INPUT,
        HIDDEN,
        OUTPUT,
        MODEL,     // Defines an embedded neural model        
        HINT       // Nodes at edges are potentially related
    }

    public enum NodeStatus
    {
        INACTIVE,
        ACTIVE        
    }
}