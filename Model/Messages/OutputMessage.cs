using System;
using System.Collections.Generic;

namespace AIkailo.Model.Internal
{
    public class OutputMessage : IMessage
    {
        public IScene Scene { get; }

        public OutputMessage(IScene scene)
        {
            Scene = scene;
        }
    }
}
