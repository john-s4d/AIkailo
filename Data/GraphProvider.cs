using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace AIkailo.Data
{
    public class GraphProvider : IDisposable
    {
        private Neo4jConnection _neo4j { get; set; }

        public GraphProvider(Neo4jConnection connection)
        {
            _neo4j = connection;
        }

        /*** Synchronous ***/

        internal void VerifyConnection()
        {
            // TODO: Catch and handle connection error
            _neo4j.VerifyConnection().Wait();
        }

        internal IRecord MergeNode(string label)
        {
            return MergeNode_Async(label).Result;
        }

        internal IRecord MergeNodeBetween(IEnumerable<ulong> startIds, IEnumerable<ulong> finishIds)//, string type)
        {
            //return MergeNodeBetween_Async(startIds, finishIds, type).Result;
            return MergeNodeBetween_Async(startIds, finishIds).Result;
        }

        internal IEnumerable<IRecord> GetEdgesFrom(IEnumerable<ulong> ids)
        {
            return GetEdgesFrom_Async(ids).Result;
        }

        internal IEnumerable<IRecord> GetEdgesTo(IEnumerable<ulong> ids)
        {
            return GetEdgesTo_Async(ids).Result;
        }

        internal IEnumerable<IRecord> GetEdgesBetween(IEnumerable<ulong> startIds, ulong finishId)
        {
            return GetEdgesBetween_Async(startIds, finishId).Result;
        }

        internal IEnumerable<IRecord> GetEdgesBetween(ulong startId, IEnumerable<ulong> finishIds)
        {
            return GetEdgesBetween_Async(startId, finishIds).Result;
        }

        /*** Asynchronous ***/

        private async Task<IRecord> MergeNode_Async(string label)
        {
            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                IResultCursor result = await tx.RunAsync(
                        GraphQuery.MERGE_NODE_FROM_LABEL,
                        new { label }
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

        private async Task<IRecord> MergeNodeBetween_Async(IEnumerable<ulong> startIds, IEnumerable<ulong> finishIds)//, string type)
        {

            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                IResultCursor result = await tx.RunAsync(
                        GraphQuery.MERGE_NODE_BETWEEN_MULTIPLE_NODES,
                        new { startIds, finishIds }//, type }
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

        internal async Task<IEnumerable<IRecord>> GetEdgesFrom_Async(IEnumerable<ulong> ids)
        {
            IAsyncSession session = _neo4j.NewAsyncSession();
            IAsyncTransaction tx = await session.BeginTransactionAsync();

            try
            {
                IResultCursor result = await tx.RunAsync(
                        GraphQuery.GET_EDGES_FROM_NODE_IDS,
                        new { ids }
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

        internal async Task<IEnumerable<IRecord>> GetEdgesTo_Async(IEnumerable<ulong> ids)
        {   
            throw new NotImplementedException();
        }

        internal async Task<IEnumerable<IRecord>> GetEdgesBetween_Async(ulong startId, IEnumerable<ulong> finishIds)
        {
            throw new NotImplementedException();
        }

        internal async Task<IEnumerable<IRecord>> GetEdgesBetween_Async(IEnumerable<ulong> startIds, ulong finishId)
        {
            throw new NotImplementedException();
        }


        /*** Interface ***/

        public void Dispose()
        {
            ((IDisposable)_neo4j).Dispose();
        }
    }
}
