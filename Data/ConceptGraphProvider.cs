using AIkailo.External.Model;
using AIkailo.Core.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace AIkailo.Data
{
    public class ConceptGraphProvider : IDataProvider
    {
        private Neo4jConnection _neo4j { get; set; }

        public ConceptGraphProvider(Neo4jConnection connection)
        {
            _neo4j = connection;
        }

        public Concept GetOrCreate(Property property)
        {
            return GetOrCreateAsync(property).Result;
        }

        public async Task<Concept> GetOrCreateAsync(Property property)
        {
            if (property == null) { throw new ArgumentNullException("property"); }

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();
           
            try
            {
                IResultCursor result = await tx.RunAsync(
                        "MERGE (c:Concept {definition: $property}) " +
                        "ON CREATE SET c.definition = $property, c.id = apoc.create.uuid() " +                        
                        "RETURN c.id, c.definition", 
                        new { property = property.ToString() }
                    );
                
                IRecord record = await result.SingleAsync();
                IResultSummary summary = result.ConsumeAsync().Result;
                
                await tx.CommitAsync(); // TODO: only if created

                Property pResult = record["c.definition"].ToString();
                string id = record["c.id"].ToString();

                Concept c = new Concept(pResult, id);
                return c;
                
            }
            catch (Exception e)
            {
                await tx.RollbackAsync();
                throw e;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public Scene GetOrCreate(Concept concept1, Concept concept2)
        {
            return GetOrCreateASync(concept1, concept2).Result;
        }

        public async Task<Scene> GetOrCreateASync(Concept concept1, Concept concept2)
        {
            if (concept1.Id == null) { throw new ArgumentNullException("concept1.Id"); }
            if (concept2.Id == null) { throw new ArgumentNullException("concept2.Id"); }

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {   // TODO: index on Concept.id
                IResultCursor result = await tx.RunAsync(
                    "MATCH (c1:Concept {id:$concept1}),(c2:Concept {id:$concept2}) " +
                    "MERGE (c1)-[a1:Tags]->(p:Concept)<-[a2:Tags]-(c2) " +
                    "ON CREATE SET p.id = apoc.create.uuid() " +
                    "RETURN c1, c2, a1, a2, p ",
                        new { concept1 = concept1.Id, concept2 = concept2.Id }
                    );

                IRecord record = await result.SingleAsync();
                await tx.CommitAsync(); // TODO: only if created  

                INode n1 = (INode)record["c1"];
                Concept c1 = new Concept(n1.Properties["definition"].ToString(), n1.Properties["id"].ToString());

                INode n2 = (INode)record["c2"];
                Concept c2 = new Concept(n2.Properties["definition"].ToString(), n2.Properties["id"].ToString());

                IRelationship a1 = (IRelationship)record["a1"];
                IRelationship a2 = (IRelationship)record["a2"];

                INode n5 = (INode)record["p"];
                Concept p = new Concept(n5.Properties.ContainsKey("definition") ? n5.Properties["definition"].ToString() : null, n5.Properties["id"].ToString());

                Scene s = new Scene(p);
                s.AddVerticesAndEdge(new ConceptEdge(c1, p, a1.Properties));
                s.AddVerticesAndEdge(new ConceptEdge(c2, p, a2.Properties));
                return s;
            }
            catch (Exception e)
            {
                await tx.RollbackAsync();
                throw e;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<Scene> GetOrCreateASync(Scene[] scenes)
        {

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {   // TODO: index on Concept.id
                IResultCursor result = await tx.RunAsync(
                    "MATCH (c:Concept) WHERE c.id IN $ids " +
                    "MERGE (c)-[a:Tags]->(p:Concept) " +
                    "ON CREATE SET p.id = apoc.create.uuid() " +
                    "RETURN c, a, p",
                        new { ids = scenes.Select(x => x.Id).ToArray() }
                    );

                IRecord record = await result.SingleAsync();
                await tx.CommitAsync(); // TODO: only if created  

                INode n1 = (INode)record["c1"];
                Concept c1 = new Concept(n1.Properties["definition"].ToString(), n1.Properties["id"].ToString());

                INode n2 = (INode)record["c2"];
                Concept c2 = new Concept(n2.Properties["definition"].ToString(), n2.Properties["id"].ToString());

                IRelationship a1 = (IRelationship)record["a1"];
                IRelationship a2 = (IRelationship)record["a2"];

                INode n5 = (INode)record["p"];
                Concept p = new Concept(n5.Properties.ContainsKey("definition") ? n5.Properties["definition"].ToString() : null, n5.Properties["id"].ToString());

                Scene s = new Scene(p);
                s.AddVerticesAndEdge(new ConceptEdge(c1, p, a1.Properties));
                s.AddVerticesAndEdge(new ConceptEdge(c2, p, a2.Properties));
                return s;
            }
            catch (Exception e)
            {
                await tx.RollbackAsync();
                throw e;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public Scene GetOrCreate(params Scene[] scenes)
        {
            return GetOrCreateASync(scenes).Result;
        }
    }
}
