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

        public string Name { get; } = "AIkailo.ObservationService";

        public AkailoServiceState State => throw new NotImplementedException();

        public Task Merge(Scene scene)
        {
            // Merge this scene into the current context
            throw new NotImplementedException();
        }

        public void Start()
        {
            /*
            Start thread(s) to:
            - mine and classify scenes in the current context, looking for known scenes
            - reduce scenes
            - resolve scenes, merge multiple scenes, then look for / request / publish action for a scene representing the missing concepts
            - publish a frame when it finds a known scene with a process model and all the required concepts

            Include functionality for:
            - decay
            - urgency, importance
            - noise filtering
            */

            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
