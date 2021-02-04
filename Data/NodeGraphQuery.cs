namespace AIkailo.Data
{
    internal class NodeGraphQuery
    {
        internal const string MERGE_NODE_FROM_LABEL = @"
            MERGE(n:Node { type:$type, label:$label })
            ON CREATE SET n.type = $type, n.label = $label, n.id = apoc.create.uuid()
            RETURN n.id, n.Features
        ";

        internal const string GET_EDGES_FROM_NODE_IDS = @"
            MATCH (n1:Node)-[c:Connection]->(n2:Node)
            WHERE n1.id IN $ids
            RETURN n1.id, c, n2.id
        ";

        internal const string CREATE_EDGE_BETWEEN_2_NODES = @"
            
        ";
    }
}