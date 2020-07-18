using AIkailo.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.External
{
    public abstract class AIkailoModule
    {

        public abstract string Name { get; }
        private BusAdapter _bus;

        public AIkailoModule(string host)
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

        public Task Publish(List<Tuple<IConvertible, IConvertible>> message)
        {
            return _bus.Publish(Name, message);
        }
    }
}
