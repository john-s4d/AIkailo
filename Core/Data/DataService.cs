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

        public IAkailoServiceState State => throw new NotImplementedException();

        //private AssociationDataProvider _associationData;
        private ConceptGraphProvider _conceptGraphProvider;

        public DataService(string dataDirectory)
        {
            _conceptGraphProvider = new ConceptGraphProvider(new Neo4jConnection("bolt://localhost"));
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        // Exact Matching

        public async Task<IConcept> GetOrCreate(Property property)
        {
            return await _conceptGraphProvider.GetOrCreate(property);
        }

        public async Task<IScene> GetOrCreate(params Property[] properties)
        {
            //if (definitions.Length == 1) { return new Scene(await GetOrCreate(definitions[0])); }
            return await _conceptGraphProvider.GetOrCreate(properties);
        }
        /*
        public async Task<IScene> GetOrCreate(params Concept[] concepts)
        {

            return await _conceptGraphProvider.GetOrCreate(concepts);
        }*/
    }
}

