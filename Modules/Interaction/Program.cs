using AIkailo.External;
using AIkailo.External.Model;
using System;
using System.Threading.Tasks;

namespace AIkailo.Modules.Interaction
{
    internal class Program
    {
        private const string HOST = @"rabbitmq://localhost";

        private static AIkailoAdapter _adapter = new AIkailoAdapter(HOST);
        private static AIkailoInput _input;

        internal static void Main(string[] args)
        {

            _input = _adapter.RegisterInput("Interaction.Input");
            _adapter.RegisterOutput("Interaction.Output", OnOutputEvent);

            Run().Wait();
        }

        private static void OnOutputEvent(FeatureVector data)
        {
            Console.WriteLine("output:> " + data[0].Item1 + " : " + data[0].Item2);
            //Console.Write("input:> ");
        }

        private async static Task Run()
        {
            _adapter.Start();

            //Console.WriteLine("output:> Enter message (or quit to exit)");
            Console.WriteLine("input:>");
            do
            {
                string value = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (value.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }

                FeatureVector data = new FeatureVector
                {
                    { "input", value }
                };

                _input.OnInputEvent(data);

                //await _input.CreateInputEvent(data);

                Console.WriteLine("sent:> " + data[0].Item1 + " : " + data[0].Item2);
            }
            while (true);

            _adapter.Stop();
        }
    }
}
