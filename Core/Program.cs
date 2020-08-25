using System;
using System.ServiceProcess;

namespace AIkailo.Core
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                new AIkailo().StartInConsole(args).Wait();
            }
            else
            {   
                ServiceBase.Run(new AIkailo());                
            }           
        }
    }
}
