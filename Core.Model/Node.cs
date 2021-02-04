using AIkailo.External.Model;

namespace AIkailo.Core.Model
{
    public class Node
    {
        public NodeType NodeType { get; set; } = NodeType.NULL;
        public string Id { get; set; } 
        public Property Label { get; set; }                
        public float Value { get; set; }        
        public FeatureArray Features { get; set; }
    }

    public enum NodeType
    {
        NULL,
        INPUT,
        HIDDEN,
        PROCESS,
        SUBNET,
        OUTPUT
    }
}