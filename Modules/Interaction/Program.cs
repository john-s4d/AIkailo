using AIkailo.External;
using AIkailo.External.Common;
using System;
using System.Threading.Tasks;

namespace AIkailo.Modules.Interaction
{
    internal class Program
    {
        private const string HOST = @"rabbitmq://localhost";

        private static AIkailoAdapter _adapter = new AIkailoAdapter(HOST);
        private static AIkailoInputInterface _inputInterface;

        private const string INPUT_NAME = "Interaction.Input";
        private const string OUTPUT_NAME = "Interaction.Output";

        internal static void Main(string[] args)
        {
            _adapter.RegisterInput(INPUT_NAME, out _inputInterface);
            _adapter.RegisterOutput(OUTPUT_NAME, Output);
            _adapter.Start();

            Train();

            Run().Wait();

            _adapter.Stop();            
        }

        private static void Output(FeatureArray data)
        {
            Console.WriteLine("output:> " + data[0].Item1 + " : " + data[0].Item2);
            //Console.Write("input:> ");
        }

        private static void Input(FeatureArray data)
        {
            _inputInterface.Input(data);
        }
      

        private static void Train()
        {
            var flow = new TrainingStep()
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

                Input(data);

                //await _input.CreateInputEvent(data);

                Console.WriteLine("sent:> " + data[0].Item1 + " : " + data[0].Item2);
            }
            while (true);

        }

       
    }
}
