using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIkailo.External;

namespace AIkailo.Modules.Interaction
{
    internal class InteractionModule : AIkailoModule
    {
        public override string Name { get; } = "Interaction";

        public InteractionModule(string host) : base(host) { }

    }
}
