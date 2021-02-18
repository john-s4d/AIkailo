namespace AIkailo.Data
{
    internal class GraphQuery
    {
        internal const string MERGE_NODE_FROM_LABEL = @"
            MERGE(n:Node { type:$type, label:$label })
            ON CREATE SET n.type = $type, n.label = $label, n.id = apoc.create.uuid()
            RETURN n.id, n.Features
        ";

        internal const string GET_EDGES_FROM_NODE_IDS = @"
            MATCH (n1:Node)-[e:Edge]->(n2:Node)
            WHERE n1.id IN $ids
            RETURN n1.id, e.id, n2.id, n2.type
        ";

        internal const string MERGE_NODE_BETWEEN_MULTIPLE_NODES = @"
            MATCH (n1:Node) WHERE n1.id IN $startIds
            MATCH (n2:Node) WHERE n2.id IN $finishIds
            MERGE (n1)-[e1:Edge]->(n:Node)-[e2:Edge]->(n2)
            ON CREATE SET n.type = $type, n.id = apoc.create.uuid(), e1.id = apoc.create.uuid(), e2.id = apoc.create.uuid()
            RETURN n.id
        ";

        /*
         internal const string MERGE_CONCEPT_FROM_DEF = @"
            MERGE(c:Concept { definition: $definition})
            ON CREATE SET c.definition = $definition, c.id = apoc.create.uuid()
            RETURN c.id, c.definition
            ";

        internal const string MERGE_COMMON_PARENT_REL_FROM_2_ID = @"
            MATCH(c1:Concept { id:$id1}),(c2:Concept {id:$id2}) 
            MERGE (c1)-[a1:TAGS]->(p:Concept)<-[a2:TAGS]-(c2) 
            ON CREATE SET p.id = apoc.create.uuid() 
            RETURN a1, a2, p
        ";

        internal const string GET_PARENT_FROM_IDS = @"            
            MATCH(c:Concept)
            WHERE c.id IN $ids
            WITH collect(c) as c
            WITH head(c) AS head, tail(c) AS tail
            MATCH (head)-[:TAGS]->(p:Concept)
            WHERE ALL(c IN tail WHERE (c)-[:TAGS]->(p))
            WITH head + tail AS cList
            UNWIND cList AS c
            MATCH (c)-[a:TAGS]->(p)
            RETURN c,a,p
        ";

        internal const string CREATE_PARENT_FROM_IDS = @"
            CREATE(p:Concept {id:apoc.create.uuid()})
            WITH p AS p
            MATCH(c:Concept) 
            WHERE c.id IN $ids
            MERGE (c)-[a:TAGS]->(p)
            ON CREATE SET a.id = apoc.create.uuid()            
            RETURN c,a,p
        ";

        internal const string CREATE_CONCEPT_NO_DEF = @"
            CREATE(c:Concept {id:apoc.create.uuid()})
            RETURN c
        ";

        internal const string CREATE_CONCEPT_WITH_DEF = @"
            CREATE(c:Concept {definition:$definition, id:apoc.create.uuid()})
            RETURN c
        ";

        internal const string MERGE_INPUT_NODE = @"
            MERGE(n:InputNode { source:$source, parameter: @parameter })
            ON CREATE SET n.source = $source, n.parameter = $parameter, n.id = apoc.create.uuid()
            RETURN n.id, n.source, n.parameter
        ";
         
         
         
         */
    }
}