using System;
using AIkailo.External.Model;
using System.Linq;
using System.Threading.Tasks;
using AIkailo.Core.Model;

namespace AIkailo.Data
{
    public sealed class DataService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.DataService";

        public AkailoServiceState State => throw new NotImplementedException();

        private ConceptGraphProvider DataProvider { get; }
        public SceneProvider SceneProvider { get; }

        public DataService(string host)
        {   
            //DataProvider = new ConceptGraphProvider(new Neo4jConnection("http://localhost:7474"));
            DataProvider = new ConceptGraphProvider(new Neo4jConnection("bolt://localhost:7687", "neo4j", "password"));
            SceneProvider = new SceneProvider(DataProvider);            
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }        
    }
}

