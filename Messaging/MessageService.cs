using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AIkailo.Messaging.Messages;
using AIkailo.Core.Model;
using MassTransit;

namespace AIkailo.Messaging
{
    public sealed class MessageService : IAIkailoService
    {
        // https://masstransit-project.com/usage/consumers.html
        // TODO: Other subscriber types: Observer, Handler, Consumer, Factories, Connect, Instance        

        public string Name { get; } = "AIkailo.MessageService";

        public IAkailoServiceState State { get; private set; } = IAkailoServiceState.STOPPED;

        private Dictionary<string, Type> _consumers = new Dictionary<string, Type>();
        private Dictionary<string, object> _instances = new Dictionary<string, object>();
        private IBusControl _bus;
        private string _host;

        public MessageService(string host)
        {
            _host = host;
        }

        public void Start()
        {
            if (State == IAkailoServiceState.STARTED)
            {
                throw new InvalidOperationException("MessageService is already started.");
            }

            _bus = Bus.Factory.CreateUsingRabbitMq(busConfig =>
            {
                busConfig.Host(_host);

                busConfig.PurgeOnStartup = true;

                foreach (string name in _consumers.Keys)
                {
                    busConfig.ReceiveEndpoint(name, e =>
                    {
                        e.Consumer(_consumers[name], type => Activator.CreateInstance(type));                        
                    });
                }

                foreach (string name in _instances.Keys)
                {
                    busConfig.ReceiveEndpoint(name, e =>
                    {
                        e.Instance(_instances[name]);                        
                    });
                }
            });
            _bus.Start();
            State = IAkailoServiceState.STARTED;
        }

        public async void Send<TMessage>(string target, TMessage message)
            where TMessage : class, IMessage
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:" + target));
            _ = endpoint.Send(message);
        }

        public void Stop()
        {
            _bus.Stop();
            _bus = null;
            State = IAkailoServiceState.STOPPED;
        }

        public Task Publish<TMessage>(TMessage message)
            where TMessage : class, IMessage
        {
            return _bus.Publish(message, typeof(TMessage));
        }

        public void RegisterConsumer<TMessage, TConsumer>(string name)
            where TMessage : class, IMessage
            where TConsumer : class, IMessageConsumer<TMessage>
        {
            _consumers.Add(name, typeof(TConsumer));
           // _consumers.Add(name + "_error", typeof(FaultConsumer<TMessage>));
        }

        public void RegisterConsumer<TMessage>(string name, IMessageConsumer<TMessage> instance)
            where TMessage : class, IMessage
        {
            _instances.Add(name, instance);
            //_consumers.Add(name + "_error", typeof(FaultConsumer<TMessage>));
        }

        public void UnregisterConsumer(string name)
        {
            _consumers.Remove(name);
            _instances.Remove(name);
        }
    }
}
