using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AIkailo.External;

namespace AIkailo.Modules.Interaction
{
    internal class Program
    {
        private const string HOST = "rabbitmq://localhost";        
        private const string MODULE_NAME = "AIkailo.Modules.Interaction"; // TODO: This should be a unique Id

        internal static InteractionModule _aikailo;

        internal static void Main(string[] args)
        {
            /*
            _aikailo = new AIkailoModuleInterface(HOST);            
            _aikailo.RegisterAction<InteractionActionMessage, InteractionActionMessageConsumer>(MODULE_NAME);            
            _aikailo.Start();
            */

            _aikailo = new InteractionModule(HOST);
            _aikailo.Start();
            
            Run().Wait();
        }      
        private async static Task Run()
        {
            Console.WriteLine("Enter message (or quit to exit)");
            Console.Write("> ");
            do
            {
                string value = await Task.Run(() =>
                {   
                    return Console.ReadLine();
                });

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    return;
                /*
                InteractionSensorMessage message = new InteractionSensorMessage()
                {
                    Data = value
                };

                //Console.WriteLine(string.Format(">> Sending message type {0} with data {1}", message.GetType().FullName,  message.Data));    
                */

                Tuple<IConvertible, IConvertible> message = new Tuple<IConvertible, IConvertible>("interaction", value);

                await _aikailo.Publish(new List<Tuple<IConvertible, IConvertible>>() { message }); // don't wait?

                Console.WriteLine(">> Message Sent");
            }
            while (true);
        }
    }
}
