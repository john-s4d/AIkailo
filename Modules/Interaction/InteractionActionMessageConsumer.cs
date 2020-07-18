using AIkailo.External;
using System;

namespace AIkailo.Modules.Interaction
{
    public class InteractionActionMessageConsumer : IActionMessageConsumer<InteractionActionMessage>
    {
        public void Consume(InteractionActionMessage message)
        {
            //Console.WriteLine(string.Format(">> Received Message For: {0}", message.TargetName));
            /*
            foreach (Tuple<IConvertible, IConvertible> pair in message.Data)
            {
                Console.WriteLine(string.Format(">> Data Pair: {0} {1}", pair.Item1.ToString(), pair.Item2.ToString()));
            }*/
            Console.WriteLine("Message Received: " + message.Data);
            //Console.WriteLine(">> End Received Message");
            Console.WriteLine("> ");
        }
    }
}
