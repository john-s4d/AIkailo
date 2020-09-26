using AIkailo.External;
using AIkailo.Model.Common;
using System;
using System.Threading.Tasks;

namespace AIkailo.Modules.Interaction
{
    internal class Program
    {
        private const string HOST = @"rabbitmq://localhost";

        private static Adapter _aikailo = new Adapter(HOST);
        private static Input _input;

        internal static void Main(string[] args)
        {

            _input = _aikailo.CreateInputSource("Interaction.Input");
            _aikailo.CreateOutputTarget("Interaction.Output", OnOutputEvent);

            Run().Wait();
        }

        private static void OnOutputEvent(DataPackage data)
        {
            Console.WriteLine("output:> " + data[0].Item1 + " : " + data[0].Item2);
            //Console.Write("input:> ");
        }

        private async static Task Run()
        {
            _aikailo.Start();

            //Console.WriteLine("output:> Enter message (or quit to exit)");
            Console.WriteLine("input:>");
            do
            {
                string value = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (value.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }

                DataPackage data = new DataPackage
                {
                    { "input", value }
                    //,{ 0, 1 }
                };

                _input.OnInputEvent(data);

                //await _input.CreateInputEvent(data);

                Console.WriteLine("sent:> " + data[0].Item1 + " : " + data[0].Item2);
            }
            while (true);

            _aikailo.Stop();
        }
    }
}
