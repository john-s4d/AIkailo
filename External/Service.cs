using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIkailo.Model;

namespace AIkailo.External
{
    public sealed class ExternalService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.ExternalService";

        public IAkailoServiceState State { get; internal set; } = IAkailoServiceState.STOPPED;

        public void Start()
        {
            State = IAkailoServiceState.STARTED;
        }

        public void Stop()
        {
            State = IAkailoServiceState.STOPPED;
        }
    }
}
