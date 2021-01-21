using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    public class ConceptGraphQuery
    {

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
    }
}
