using System;
using System.Collections.Generic;
using AIkailo.Core.Model;

namespace AIkailo.Messaging.Messages
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
