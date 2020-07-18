using System;
using System.ServiceProcess;
using AIkailo.Data;

namespace AIkailo.Core
{
    public partial class AIkailoService : ServiceBase
    {
        private DataService _data = new DataService();

        public AIkailoService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // If needed to attach debugger to Service
            // Thread.Sleep(20000); 

            Console.WriteLine("AIkailo.Core.OnStart()");
            _data.Start();
        }

        protected override void OnStop()
        {
            _data.Stop();
        }

        internal void StartInConsole(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }
    }
}
