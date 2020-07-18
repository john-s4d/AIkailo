using System;
using AIkailo.External;
using AIkailo.Messaging;
//using AIkailo.Internal;

namespace AIkailo.External
{
    public class SensorMessageConsumer : IMessageConsumer<ISensorMessage>
    {
        public void Consume(ISensorMessage message)
        {
            Console.WriteLine("SensorMessageConsumer.Consume");
            
            //DataService.Consume(message);
            
        }
    }
}
