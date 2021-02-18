using AIkailo.Core.Common;
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
        internal INodeProvider NodeFactory { get; private set; }
        internal IExternalProvider ExternalProvider { get; private set; }
        public Context DefaultContext { get; private set; }
        public Trainer Trainer { get; private set; }

        public ExecutiveService(INodeProvider nodeFactory, IExternalProvider externalProvider)        
        {
            NodeFactory = nodeFactory;
            ExternalProvider = externalProvider;

            DefaultContext = new Context(NodeFactory, ExternalProvider, null);
            Trainer = new Trainer(NodeFactory);
        }

        public void Incoming(Node node)
        {
            DefaultContext.Incoming(node);
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
