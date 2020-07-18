using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.Internal;
using System;

namespace AIkailo.Data
{
    public class DataService
    {
        private const string HOST = @"rabbitmq://localhost";
        private const string DATA_DIRECTORY = @"G:\AIkailo\Data\";

        //private const string SENSOR_QUEUE_NAME = "sensor_queue";
        private const string REDUCE_QUEUE_NAME = "reduce_queue";
        private const string EXPAND_QUEUE_NAME = "expand_queue";
        //private const string SENSOR_FAULT_QUEUE = "sensor_fault_queue";

        private BusAdapter _bus;
        private DataManager _data;

        private static DataService _instance;

        public DataService()
        {
            _instance = this;

            _data = new DataManager(DATA_DIRECTORY);

            _bus = new BusAdapter(HOST);

            _bus.RegisterAction<ReduceMessage, ReduceMessageConsumer>(REDUCE_QUEUE_NAME);
            _bus.RegisterAction<ExpandMessage, ExpandMessageConsumer>(EXPAND_QUEUE_NAME);

            //_messageBus.RegisterAction<ISensorMessage, IMessageConsumer<ISensorMessage>>(SENSOR_QUEUE_NAME);

            //_messageBus.RegisterAction(REDUCE_QUEUE_NAME, typeof(ReduceMessageConsumer));
            //_messageBus.AddAction(EXPAND_QUEUE_NAME, typeof(ExpandMessageConsumer));

            //_messageBus.AddConsumer<SensorMessageFaultConsumer>(SENSOR_FAULT_QUEUE, typeof(SensorMessageFaultConsumer));
        }

        public void Start()
        {
            _bus.Start();
        }

        public void Stop()
        {
            _bus.Stop();
        }
        internal static void Consume(ReduceMessage message)
        {
            throw new NotImplementedException();
        }

        internal static void Consume(ExpandMessage message)
        {
            throw new NotImplementedException();
        }

        /*
        internal static Task Publish<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            return _instance._bus.Publish(message);
        }*/

        internal static DataManager Data
        {
            get { return _instance._data; }
        }
    }
}
