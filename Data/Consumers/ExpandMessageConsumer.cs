using AIkailo.Internal;
using AIkailo.Messaging;
using System;

namespace AIkailo.Data
{
    public class ExpandMessageConsumer : IMessageConsumer<ExpandMessage>
    {
        public void Consume(ExpandMessage message)
        {
            Console.WriteLine("ExpandMessageConsumer.Consume(ExpandMessage)");
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
