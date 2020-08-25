using AIkailo.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Modules.Interaction
{
    internal class Program
    {   
        private const string HOST = @"rabbitmq://localhost";

        private static Adapter _aikailoAdapter = new Adapter(HOST);
        private static Input _input = new Input("Interaction.Input");
        private static Output _output = new Output("Interaction.Output");

        internal static void Main(string[] args)
        {
            _aikailoAdapter.AddOutput(_output);
            _output.OutputEvent += OnOutputEvent;

            _aikailoAdapter.AddInput(_input);

            Run().Wait();
        }

        private static void OnOutputEvent(List<Tuple<IConvertible, IConvertible>> data)
        {
            Console.WriteLine("output:> " + data[0].Item1 + " : " + data[0].Item2);
            //Console.Write("input:> ");
        }

        private async static Task Run()
        {
            _aikailoAdapter.Start();            

            //Console.WriteLine("output:> Enter message (or quit to exit)");
            Console.WriteLine("input:>");
            do
            {
                string value = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (value.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }

                Tuple<IConvertible, IConvertible> data = new Tuple<IConvertible, IConvertible>((IConvertible)"input", (IConvertible)value);

                _input.CreateInputEvent(new List<Tuple<IConvertible, IConvertible>> { data });

                //await _input.SensorEvent(new List<Tuple<IConvertible, IConvertible>> { data });

                Console.WriteLine("sent:> " + data.Item1 + " : " + data.Item2);
            }
            while (true);

            _aikailoAdapter.Stop();            
        }
    }
}
