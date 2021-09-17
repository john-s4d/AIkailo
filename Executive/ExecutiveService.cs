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
        //private INodeProvider _nodeProvider;
        private ITimeProvider _timeProvider;
        private Agent _primaryAgent;

        public Trainer Trainer { get; private set; }

        public ExecutiveService(INodeProvider nodeProvider)        
        {
            _timeProvider = new TimeProvider();
            _primaryAgent = new Agent(nodeProvider, _timeProvider);
        }

        public void Input(IEnumerable<INeuron> neurons)
        {
            if (State != AkailoServiceState.STARTED)
            {
                throw new InvalidOperationException();
            }
            _primaryAgent.Input(neurons);
        }

        public void Start() 
        {
             _timeProvider.Start();
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            _timeProvider.Stop();
            State = AkailoServiceState.STOPPED;
        }
    }
}
