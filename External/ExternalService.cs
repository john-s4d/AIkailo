using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.External
{
    public sealed class ExternalService : IAIkailoService, IExternalProvider
    {        
        public string Name { get; } = "AIkailo.ExternalService";

        public AkailoServiceState State { get; internal set; } = AkailoServiceState.STOPPED;

        public void Start()
        {
            State = AkailoServiceState.STARTED;
        }

        public void Stop()
        {
            State = AkailoServiceState.STOPPED;
        }       
    }
}
