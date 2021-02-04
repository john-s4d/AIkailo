using AIkailo.External.Model;
using AIkailo.Core.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace AIkailo.Data
{
    public class NodeGraphProvider : IDataProvider
    {
        private Neo4jConnection _neo4j { get; set; }

        public NodeGraphProvider(Neo4jConnection connection)
        {
            _neo4j = connection;
        }

        public void Load(ref Node node)
        {   
            if (node.Label == null) { throw new ArgumentNullException("node.Label"); }

            IRecord record = GetOrCreateAsync(node.NodeType, node.Label).Result;
            node.Id = record["n.id"].ToString();
            //node.Features = (FeatureArray)record["n.Features"];
        }

        public async Task<IRecord> GetOrCreateAsync(NodeType type, Property label)
        {
            if (label == null) { throw new ArgumentNullException("label"); }

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();
           
            try
            {
                IResultCursor result = await tx.RunAsync(
                        NodeGraphQuery.MERGE_NODE_FROM_LABEL,
                        new { type = type.ToString(), label = label.ToString() }
                    ); ;
                
                IRecord record = await result.SingleAsync();
                IResultSummary summary = result.ConsumeAsync().Result;
                
                await tx.CommitAsync(); // TODO: only if created
                
                return record;                
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

        public IEnumerable<Connection> GetEdges(IEnumerable<Node> activeNodes)
        {
            Dictionary<string, Node> nodesById = new Dictionary<string, Node>();
            foreach(Node n in activeNodes)
            {
                nodesById.Add(n.Id, n);
            }
            List<Connection> result = new List<Connection>();

            foreach (IRecord record in GetEdgesAsync(activeNodes).Result)
            {
                Connection c = new Connection();
                c.Id = (string)record["c.id"];
                c.Features = (FeatureArray)record["c.features"];
                c.Source = nodesById[(string)record["n1.id"]];                
                c.Target = nodesById.ContainsKey((string)record["n2.id"]) ? nodesById[(string)record["n2.id"]] : new Node() { Id = (string)record["n2.id"] };
                result.Add(c);
            }
            return result;
        }

        public async Task<IEnumerable<IRecord>> GetEdgesAsync(IEnumerable<Node> activeNodes)
        {
            if (activeNodes == null) { throw new ArgumentNullException(nameof(activeNodes)); }

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                IResultCursor result = await tx.RunAsync(
                        NodeGraphQuery.GET_EDGES_FROM_NODE_IDS,
                        new { ids = activeNodes.Select(x => x.Id).ToArray() }
                    );

                return await result.ToListAsync();                
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

        public void Dispose()
        {
            ((IDisposable)_neo4j).Dispose();
        }

        /*
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

                Neo4j.Driver.INode n5 = (Neo4j.Driver.INode)record["p"];
                Concept p = new Concept()
                {
                    Label = n5.Properties.ContainsKey("definition") ? n5.Properties["definition"].ToString() : null,
                    Id = n5.Properties["id"].ToString()
                };

                Scene s = new Scene(p);
                s.AddVerticesAndEdge(new Connection(concept1, p, a1.Properties));
                s.AddVerticesAndEdge(new Connection(concept2, p, a2.Properties));
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

                Neo4j.Driver.INode np = (Neo4j.Driver.INode)records[0]["p"];
                Concept p = new Concept() {
                    Label = np.Properties.ContainsKey("definition") ? np.Properties["definition"].ToString() : null,
                    Id = np.Properties["id"].ToString()
                    };

                Scene result = new Scene(p);

                foreach (Scene s in scenes)
                {
                    result.AddVerticesAndEdgeRange(s.Edges);
                }

                foreach (IRecord record in records)
                {
                    Neo4j.Driver.INode n = (Neo4j.Driver.INode)record["c"];
                    Concept c = new Concept(){
                        Label = n.Properties.ContainsKey("definition") ? n.Properties["definition"].ToString() : null,
                        Id = n.Properties["id"].ToString()
                        };

                    IRelationship a = (IRelationship)record["a"];

                    result.AddVerticesAndEdge(new Connection(c, p, a.Properties));
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
        }*/


    }
}
