using AIkailo.Common;
using AIkailo.External.Common;
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
        public Agent PrimaryAgent { get; private set; }

        private ITimeProvider _time;        

        public ExecutiveService(INeuronProvider nodeProvider)        
        {
            _time = new TimeProvider();
            PrimaryAgent = new Agent(nodeProvider, _time);
        }        

        public void Start() 
        {
            _time.Start();
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            _time.Stop();
            State = AkailoServiceState.STOPPED;
        }
    }
}
