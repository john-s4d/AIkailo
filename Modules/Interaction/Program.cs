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

        private const string INPUT_NAME = "Interaction.Input";
        private const string OUTPUT_NAME = "Interaction.Output";

        internal static void Main(string[] args)
        {

            _input = _adapter.RegisterInput(INPUT_NAME);
            _adapter.RegisterOutput(OUTPUT_NAME, OnOutputEvent);

            _adapter.Start();
            //TrainAdapter();
            

            Run().Wait();
            
        }

        private static void OnOutputEvent(FeatureArray data)
        {
            Console.WriteLine("output:> " + data[0].Item1 + " : " + data[0].Item2);
            //Console.Write("input:> ");
        }

        private static void TrainAdapter()
        {
            var flow = new DataFlow()
            {
                Source = INPUT_NAME,
                Input = new FeatureArray { { "input", "foo" } },
                
                Target = OUTPUT_NAME,
                Output = new FeatureArray { { "output", "bar" } }
            };

            _adapter.Train(flow);

        }

        private async static Task Run()
        {

            

            //Console.WriteLine("output:> Enter message (or quit to exit)");
            Console.WriteLine("input:>");
            do
            {
                string value = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (value.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }

                FeatureArray data = new FeatureArray
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
