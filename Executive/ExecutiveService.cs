using AIkailo.Common;
using AIkailo.Neural.Core;
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
        internal INodeProvider NodeProvider { get; private set; }
        internal IExternalProvider ExternalProvider { get; private set; }

        public Context Context { get; private set; }
        public Trainer Trainer { get; private set; }

        public ExecutiveService(INodeProvider nodeProvider, IExternalProvider externalProvider)        
        {
            NodeProvider = nodeProvider;
            ExternalProvider = externalProvider;

            Context = new Context(NodeProvider, ExternalProvider);
            Trainer = new Trainer(NodeProvider);
        }

        public void Incoming(IEnumerable<Node> nodes)
        {
            Context.Incoming(nodes);
        }

        public void Start() 
        {
            Context.Start();
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            Context.Stop();
            State = AkailoServiceState.STOPPED;
        }
    }
}
