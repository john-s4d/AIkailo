using AIkailo.Model;
using AIkailo.Messaging;
using System;
using System.Threading.Tasks;
using MassTransit;

namespace AIkailo.Core
{
    internal class NormalizeMessageConsumer : IMessageConsumer<NormalizeMessage>
    {
     
        public Task Consume(ConsumeContext<NormalizeMessage> context)
        {
            Console.WriteLine("NormalizeMessageConsumer.Consume(NormalizeMessage)");
            throw new NotImplementedException();

            /*
            // Disassemble the message and then publish it to the action bus
            Bus.Publish(new ActionMessage()
            {
                Data = DataService.Data.Disassemble(out string targetName, message.Scene),
                TargetName = targetName
            });*/
        }
    }
}
