using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public sealed class ExecutiveService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.ExecutiveService";        
        public AkailoServiceState State { get; private set; }
        
        internal IDataProvider DataProvider { get; private set; }

        internal IExternalProvider ExternalProvider { get; private set; }

        internal Context DefaultContext { get; private set; }

        internal Trainer Trainer { get; private set; }

        public ExecutiveService(IDataProvider dataProvider, IExternalProvider externalProvider)        
        {
            DataProvider = dataProvider;
            ExternalProvider = externalProvider;

            DefaultContext = new Context(DataProvider, ExternalProvider, null);
            Trainer = new Trainer(DataProvider);
        }

        public Task Incoming(Node node)
        {
            return DefaultContext.Incoming(node);
        }

        public Task Train(List<Node> input, List<Node> output)
        {
            return Trainer.Train(input, output);
        }

        public void Start() 
        {
            
            DefaultContext.Start();
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            DefaultContext.Stop();
            State = AkailoServiceState.STOPPED;
        }

        
    }
}
