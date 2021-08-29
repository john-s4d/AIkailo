using System;
//using AIkailo.External.Common;
using System.Linq;
using System.Threading.Tasks;
using AIkailo.Common;
using System.Collections.Generic;

namespace AIkailo.Data
{
    public sealed class DataService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.DataService";
        public AkailoServiceState State { get; private set; }
        public INodeProvider NodeProvider { get; private set; }

        private GraphProvider _dataProvider;

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
            _dataProvider = new GraphProvider(new Neo4jConnection(_host, _username, _password));
            _dataProvider.VerifyConnection();

            NodeProvider = new NodeProvider(_dataProvider);

            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            _dataProvider.Dispose();
            _dataProvider = null;

            State = AkailoServiceState.STOPPED;
        }        
    }
}

