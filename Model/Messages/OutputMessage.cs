using System;
using System.Collections.Generic;

namespace AIkailo.Model.Internal
{
    public class OutputMessage : IMessage
    {
        public Scene Scene { get; }

        public OutputMessage(Scene scene)
        {
            Scene = scene;
        }
    }
}
