using System;
using System.Threading.Tasks;
using AIkailo.Messaging;

namespace AIkailo.External
{
    public class AIkailoModuleInterface
    {
        private BusAdapter _bus;

        public AIkailoModuleInterface(string host)
        {
            _bus = new BusAdapter(host);
        }

        public void Start()
        {
            _bus.Start();
        }

        public void Stop()
        {
            _bus.Stop();
        }

        public Task Publish<TMessage>(TMessage message) where TMessage : class, ISensorMessage
        {
            return _bus.Publish(message);
        }

        public void RegisterAction<TMessage, TConsumer>(string name) where TMessage : class, IActionMessage
                                                                     where TConsumer : class, IMessageConsumer<TMessage>
        {   
            _bus.RegisterAction<TMessage, TConsumer>(name);
        }
    }
}
