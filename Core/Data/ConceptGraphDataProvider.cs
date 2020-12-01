using AIkailo.External.Model;
using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace AIkailo.Data
{
    internal class ConceptGraphProvider
    {
        private Neo4jConnection _neo4j { get; set; }

        public ConceptGraphProvider(Neo4jConnection connection)
        {
            _neo4j = connection;
        }

        public async Task<Concept> GetOrCreate(Property definition, uint depth = 0)
        {   
            if (depth != 0) throw new NotImplementedException(); // TODO

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                IResultCursor result = await tx.RunAsync(
                        "MERGE (c:Concept {definition: $definition}) " +
                        "ON CREATE SET c.definition = $definition, c.counter = 0 " +
                        "ON MATCH SET c.counter = coalesce(c.counter, 0) + 1 " +
                        "RETURN c",
                    new Dictionary<string, string> { { "definition", definition } }
                    );

                IRecord record = await result.SingleAsync();

                return null; // FIXME
                //return new Concept((string)record["definition"], (ulong)record["id"]);                
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

        // Get an exact definition
        public async Task<Scene> GetOrCreate(Property[] childDefinitions, uint depth = 0)
        {       
            if (depth != 0) throw new NotImplementedException(); // TODO

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                throw new NotImplementedException();
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
    }
}
