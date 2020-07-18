using System;
using System.Collections.Generic;
using AIkailo.Messaging;

namespace AIkailo.External
{
    public interface IActionMessage : IMessage
    {
        string Data { get; set; }
    }
}
