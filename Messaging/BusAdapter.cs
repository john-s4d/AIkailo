using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;

namespace AIkailo.Messaging
{
    public class BusAdapter
    {
        // https://masstransit-project.com/usage/consumers.html
        // TODO: Other subscriber types: Observer, Handler, Consumer, Factories, Connect, Instance
        // TODO: Handle Undeliverable

        private Dictionary<string, Type> _consumers = new Dictionary<string, Type>();
        private Dictionary<string, Type> _sensors = new Dictionary<string, Type>();
        private IBusControl _bus;
        private string _host;

        public BusAdapter(string host)
        {
            _host = host;
        }

        public void Start()
        {
            
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

                foreach (string name in _sensors.Keys)
                {
                    
                }

            });
            _bus.Start();
        }

        public void Stop()
        {
            _bus.Stop();
        }

        public void RegisterAction<TMessage, TConsumer>(string name) where TMessage: class, IMessage
                                                                     where TConsumer : class, IMessageConsumer<TMessage>
        {
            //throw new NotImplementedException();
            _consumers.Add(name, typeof(TConsumer));
        }

        public void RegisterSensor<TMessage>(string name) where TMessage : class, IMessage
        {
            //throw new NotImplementedException();
            _sensors.Add(name, typeof(TMessage));
        }

        public Task Publish<TMessage>(TMessage message) where TMessage : class, IMessage
        {   
            return _bus.Publish(message, typeof(TMessage));
        }

        public Task Publish(string Name, List<Tuple<IConvertible,IConvertible>> data)
        {
            throw new NotImplementedException();
        }
    }
}
