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
        
        internal IDataProvider NodeProvider { get; private set; }

        internal Context DefaultContext { get; private set; }

        public ExecutiveService(IDataProvider nodeProvider)        
        {
            NodeProvider = nodeProvider;            
        }

        public Task Merge(Node node)
        {
            return DefaultContext.Merge(node);
        }

        public void Start() 
        {
            DefaultContext = new Context(NodeProvider);
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
