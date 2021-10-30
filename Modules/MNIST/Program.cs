using AIkailo.External;
using AIkailo.External.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIkailo.Modules.MNIST
{
    internal class Program
    {
        
        private const string HOST = @"rabbitmq://localhost";

        private const string TRAIN_LABELS = @"C:\Users\john\Development\AIkailo\Modules\MNIST\Resources\train-labels-idx1-ubyte.gz";
        private const string TRAIN_IMAGES = @"C:\Users\john\Development\AIkailo\Modules\MNIST\Resources\train-images-idx3-ubyte.gz";
        private const string TEST_LABELS = @"C:\Users\john\Development\AIkailo\Modules\MNIST\Resources\t10k-labels-idx1-ubyte.gz";
        private const string TEST_IMAGES = @"C:\Users\john\Development\AIkailo\Modules\MNIST\Resources\t10k-images-idx3-ubyte.gz";

        private static AIkailoAdapter _adapter = new AIkailoAdapter(HOST);
        private static AIkailoInputInterface _inputInterface;

        private const string INPUT_NAME = "MNIST.Input";
        private const string OUTPUT_NAME = "MNIST.Output";

        private static Random _random = new Random();

        internal static void Main(string[] args)
        {
            _adapter.RegisterInput(INPUT_NAME, out _inputInterface);
            _adapter.RegisterOutput(OUTPUT_NAME, Output);
            _adapter.Start();

            //Train();

            Run().Wait();

            _adapter.Stop();
        }

        private static void Output(Dictionary<ulong,float> data)
        {
            Output("== Begin Output ==");
            foreach(ulong neuronId in data.Keys)
            {
                Output(neuronId + ":" + data[neuronId]);
            }
            Output("== End Output ==");
        }

        private static void Output(string message)
        {
            Console.Write(message);
        }

        private static void Input(Dictionary<ulong, float> data)
        {
            _inputInterface.Input(data);
        }

        private static void Train(string command)
        {   
            bool random = command.Contains("random");
            int count = command.Contains("all") ? 60000 : int.TryParse(command.Substring(command.LastIndexOf(' ')), out count) ? count : 10;

            Console.WriteLine("Loading Dataset..");
            List<TestCase> dataSet = new List<TestCase>(FileReaderMNIST.LoadImagesAndLables(TRAIN_LABELS, TRAIN_IMAGES));
            Console.WriteLine("Done");

            for (int i = 0; i < count; i++)
            {
                int index = random ? _random.Next(0, dataSet.Count) : i;
                TestCase dataItem = dataSet[index];

                var data = new TrainingData()
                {
                    Source = INPUT_NAME,
                    Input = dataItem.ImageAsDictionary(),

                    Target = OUTPUT_NAME,
                    Output = dataItem.LabelAsDictionary()

                };

                _adapter.Train(data);

                Console.WriteLine("Trained: " + dataItem.Label.ToString());
            }
        }

        private static void Test(string value)
        {
            throw new NotImplementedException();
        }

        

        /*
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
        }*/

        private async static Task Run()
        {

            //Console.WriteLine("output:> Enter message (or quit to exit)");
            Console.WriteLine("input:>");
            do
            {
                string command = await Task.Run(() =>
                {
                    return Console.ReadLine();
                });

                if (command.Equals("quit", StringComparison.OrdinalIgnoreCase)) { break; }

                if (command.StartsWith("train", StringComparison.OrdinalIgnoreCase))
                {
                    Train(command);
                }

                if (command.StartsWith("test", StringComparison.OrdinalIgnoreCase))
                {
                    Test(command);
                }

                /*
                FeatureArray data = new FeatureArray
                {
                    { "input", value }
                };

                Input(data);

                //await _input.CreateInputEvent(data);

                Console.WriteLine("sent:> " + data[0].Item1 + " : " + data[0].Item2);
                */
            }
            while (true);

        }

        
    }
}
