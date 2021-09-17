using AIkailo.Common;
using AIkailo.Neural.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public class Agent
    {
        private readonly Network _network;

        internal Agent(INodeProvider nodeProvider, ITimeProvider timeProvider)             
        {
            _network = new Network(nodeProvider);
            timeProvider.Tick += _network.OnTick;
        }

        internal void Input(IEnumerable<INeuron> neurons)
        {
            _network.Input(neurons);
        }
    }
}