using System;
using System.ServiceProcess;

namespace AIkailo.Core
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                AIkailoService aikailo = new AIkailoService();
                aikailo.StartInConsole(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new AIkailoService()
                };
                ServiceBase.Run(ServicesToRun);
            }
           
        }


    }
}
