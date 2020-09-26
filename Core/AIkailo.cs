using System;
using System.ServiceProcess;
using System.Collections.Generic;
using AIkailo.External;
using AIkailo.Data;
using AIkailo.Model.Internal;
using AIkailo.Messaging;
using AIkailo.Executive;
using System.Threading.Tasks;

namespace AIkailo.Core
{
    internal class AIkailo : ServiceBase
    {
        //private const string AIKAILO = "AIkailo";
        //private const string ACTION_QUEUE_NAME = "action_queue";
        //private const string ASSEMBLE_QUEUE_NAME = "assemble_queue";
        //private const string DISASSEMBLE_QUEUE_NAME = "disassemble_queue";
        //private const string CLASSIFY_QUEUE_NAME = "classify_queue"; 

        private const string DATA_DIRECTORY = @"G:\AIkailo\Data\";
        private const string MQ_HOST = @"rabbitmq://localhost";
        public static MessageService MessageService { get; } = new MessageService(MQ_HOST);
        public static DataService DataService { get; } = new DataService(DATA_DIRECTORY);
        public static ObservationService ObservationService { get; } = new ObservationService();
        public static ExternalService ExternalService { get; } = new ExternalService();        

        internal AIkailo() : base()
        {
            ServiceName = "AIkailo";            
            InitializeConsumers();
        }

        private void InitializeConsumers()
        {   
            MessageService.RegisterConsumer<InputMessage, InputMessageConsumer>("Core.Input");
            //MessageService.RegisterConsumer<ClassifyMessage, ClassifyMessageConsumer>("Core.Classify");            
            MessageService.RegisterConsumer<OutputMessage, OutputMessageConsumer>("Core.Output");
        }

        protected override void OnStart(string[] args)
        {
            // If needed to attach debugger to Service
            // Thread.Sleep(20000); 

            MessageService.Start();
            //DataService.Start();
            //ObservationService.Start();
            //ExternalService.Start();
        }

        protected override void OnStop()
        {
            MessageService.Stop();
            //DataService.Stop();
            //ObservationService.Start();
            //ExternalService.Stop();
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

                //Tuple<IConvertible, IConvertible> data = new Tuple<IConvertible, IConvertible>("input", value);

                //_input.CreateInputEvent(new List<Tuple<IConvertible, IConvertible>> { data });

                //await _input.SensorEvent(new List<Tuple<IConvertible, IConvertible>> { data });

                //Console.WriteLine("sent:> " + data.Item1 + " : " + data.Item2);
            }
            while (true);
            OnStop();
        }
    }
}
