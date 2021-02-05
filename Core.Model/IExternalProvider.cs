using AIkailo.Core.Model;
using AIkailo.External.Model;
using AIkailo.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIkailo.Core.Model
{
    public interface IExternalProvider
    {
        //Task Publish<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}