using System;
using System.ServiceProcess;
using System.Collections.Generic;
using AIkailo.External;
using AIkailo.Data;
using AIkailo.Common;
using AIkailo.Messaging;
using AIkailo.Executive;
using System.Threading.Tasks;
using AIkailo.Messaging.Messages;

namespace AIkailo.Core
{
    internal class AIkailo : ServiceBase
    {
        private const string MQ_HOST = @"rabbitmq://localhost";

        private const string DATA_DIRECTORY = @"G:\AIkailo\Data\";
        private const string DATA_HOST = @"bolt://localhost:7687";
        private const string DATA_UN = @"neo4j";
        private const string DATA_PW = @"password";
        
        public static MessageService MessageService { get; private set; } 
        public static DataService DataService { get; private set; } 
        public static ExecutiveService ExecutiveService { get; private set; }
        public static ExternalService ExternalService { get; private set;  }  

        internal AIkailo() : base()
        {
            ServiceName = "AIkailo";
        }

        protected override void OnStart(string[] args)
        {
            // If needed to attach debugger to Service
            // Thread.Sleep(20000); 

            DataService = new DataService(DATA_DIRECTORY, DATA_HOST, DATA_UN, DATA_PW);
            DataService.Start();

            MessageService = new MessageService(MQ_HOST);
            MessageService.RegisterConsumer<InputMessage, InputMessageConsumer>("Core.Input");
            MessageService.RegisterConsumer<TrainingMessage, TrainingMessageConsumer>("Core.Training");
            MessageService.Start();

            ExternalService = new ExternalService();
            ExternalService.Start();

            //ExecutiveService = new ExecutiveService(DataService.NodeProvider, ExternalService);
            ExecutiveService = new ExecutiveService(DataService.NodeProvider);
            ExecutiveService.Start();            
        }

        protected override void OnStop()
        {
            //ExternalService.Stop();
            ExecutiveService.Stop();
            DataService.Stop();
            MessageService.Stop();
        }

        internal async Task StartInConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("== AIkailo ==");
            do
            {
                string value = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (value.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }
            }
            while (true);
            OnStop();
        }
    }
}
