using System;
using AIkailo.External.Model;
using System.Linq;
using System.Threading.Tasks;
using AIkailo.Core.Model;
using System.Collections.Generic;

namespace AIkailo.Data
{
    public sealed class DataService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.DataService";

        public AkailoServiceState State { get; private set; }
        public IDataProvider DataProvider { get; private set; }

        private readonly string _directory;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;

        public DataService(string directory, string host, string username, string password)
        {
            _directory = directory;
            _host = host;
            _username = username;
            _password = password;
        }

        public void Start()
        {
            DataProvider = new NodeGraphProvider(new Neo4jConnection(_host, _username, _password));            
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            DataProvider.Dispose();
            DataProvider = null;
            State = AkailoServiceState.STOPPED;
        }        
    }
}

