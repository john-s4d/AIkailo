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

        public async Task<Concept> GetOrCreateAsync(Property definition)
        {
            if (definition == null) { throw new ArgumentNullException("definition"); }

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();
           
            try
            {
                IResultCursor result = await tx.RunAsync(
                        ConceptGraphQuery.MERGE_CONCEPT_FROM_DEF, 
                        new { definition = definition.ToString() }
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
                    ConceptGraphQuery.MERGE_COMMON_PARENT_REL_FROM_2_ID,
                        new { id1 = concept1.Id, id2 = concept2.Id }
                    );

                IRecord record = await result.SingleAsync();
                await tx.CommitAsync(); // TODO: only if created  

                //INode n1 = (INode)record["c1"];
                //Concept c1 = new Concept(n1.Properties["definition"].ToString(), n1.Properties["id"].ToString());

                //INode n2 = (INode)record["c2"];
                //Concept c2 = new Concept(n2.Properties["definition"].ToString(), n2.Properties["id"].ToString());

                IRelationship a1 = (IRelationship)record["a1"];
                IRelationship a2 = (IRelationship)record["a2"];

                INode n5 = (INode)record["p"];
                Concept p = new Concept(n5.Properties.ContainsKey("definition") ? n5.Properties["definition"].ToString() : null, n5.Properties["id"].ToString());

                Scene s = new Scene(p);
                s.AddVerticesAndEdge(new ConceptEdge(concept1, p, a1.Properties));
                s.AddVerticesAndEdge(new ConceptEdge(concept2, p, a2.Properties));
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
            {
                string[] ids = scenes.Select(x => x.Id).ToArray();

                IResultCursor resultCursor = await tx.RunAsync(
                    ConceptGraphQuery.GET_PARENT_FROM_IDS,
                        new { ids }
                    );
                
                List<IRecord> records = await resultCursor.ToListAsync();
                
                // If the parent doesn't exist, create it now and link it
                if (records.Count == 0)
                {
                    resultCursor = await tx.RunAsync(
                    ConceptGraphQuery.CREATE_PARENT_FROM_IDS,
                        new { ids }
                    );
                    records = await resultCursor.ToListAsync();
                }  

                await tx.CommitAsync(); // TODO: only if created  

                INode np = (INode)records[0]["p"];
                Concept p = new Concept(np.Properties.ContainsKey("definition") ? np.Properties["definition"].ToString() : null, np.Properties["id"].ToString());

                Scene result = new Scene(p);

                foreach (Scene s in scenes)
                {
                    result.AddVerticesAndEdgeRange(s.Edges);
                }

                foreach(IRecord record in records)
                {
                    INode n = (INode)record["c"];
                    Concept c = new Concept(n.Properties.ContainsKey("definition") ? n.Properties["definition"].ToString() : null, n.Properties["id"].ToString());

                    IRelationship a = (IRelationship)record["a"];

                    result.AddVerticesAndEdge(new ConceptEdge(c, p, a.Properties));
                }
               
                return result;
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

        public void Dispose()
        {
            ((IDisposable)_neo4j).Dispose();
        }
    }
}
