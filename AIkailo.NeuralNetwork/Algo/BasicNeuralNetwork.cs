using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace AIkailo.Executive
{
    // Source: https://towardsdatascience.com/building-a-neural-network-framework-in-c-16ef56ce1fef

    internal class BasicNeuralNetwork //: ProcessModel
    {

        private Random _random = new Random(); // TODO: Use better Random Generator

        //fundamental 
        private int[] _layers;//layers
        private float[][] _neurons;//neurons
        private float[][] _biases;//biasses
        private float[][][] _weights;//weights
        private int[] _activations;//layers

        //genetic
        public float Fitness = 0;//fitness

        //backprop
        public float LearningRate = 0.01f; //learning rate
        public float Cost = 0;

        private float[][] _deltaBiases; //biasses
        private float[][][] _deltaWeights; //weights
        private int _deltaCount;

        public bool CanBackPropagate { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
        public bool CanMutate { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public BasicNeuralNetwork()
        {

        }

        public BasicNeuralNetwork(int[] layers, string[] layerActivations)
        {
            _layers = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                _layers[i] = layers[i];
            }
            _activations = new int[layers.Length - 1];
            for (int i = 0; i < layers.Length - 1; i++)
            {
                string action = layerActivations[i];
                switch (action)
                {
                    case "sigmoid":
                        _activations[i] = 0;
                        break;
                    case "tanh":
                        _activations[i] = 1;
                        break;
                    case "relu":
                        _activations[i] = 2;
                        break;
                    case "leakyrelu":
                        _activations[i] = 3;
                        break;
                    default:
                        _activations[i] = 2;
                        break;
                }
            }
            InitNeurons();
            InitBiases();
            InitWeights();
        }


        //create empty storage array for the neurons in the network.
        private void InitNeurons() 
        {
            List<float[]> neuronsList = new List<float[]>();
            for (int i = 0; i < _layers.Length; i++)
            {
                neuronsList.Add(new float[_layers[i]]);
            }
            _neurons = neuronsList.ToArray();
        }

        //initializes random array for the biases being held within the network.
        private void InitBiases() 
        {
            List<float[]> biasList = new List<float[]>();
            for (int i = 0; i < _layers.Length; i++)
            {
                float[] bias = new float[_layers[i]];
                for (int j = 0; j < _layers[i]; j++)
                {
                    bias[j] = RandomRange(-0.5f, 0.5f);
                }
                biasList.Add(bias);
            }
            _biases = biasList.ToArray();
        }

        //initializes random array for the weights being held in the network.
        private void InitWeights()
        {
            List<float[][]> weightsList = new List<float[][]>();
            for (int i = 1; i < _layers.Length; i++)
            {
                List<float[]> layerWeightsList = new List<float[]>();
                int neuronsInPreviousLayer = _layers[i - 1];
                for (int j = 0; j < _neurons[i].Length; j++)
                {
                    float[] neuronWeights = new float[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = RandomRange(-0.5f, 0.5f);
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            _weights = weightsList.ToArray();
        }

        //feed forward, inputs >==> outputs.
        public float[] FeedForward(float[] inputs) 
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                _neurons[0][i] = inputs[i];
            }
            for (int i = 1; i < _layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < _neurons[i].Length; j++)
                {
                    float value = 0f;
                    for (int k = 0; k < _neurons[i - 1].Length; k++)
                    {
                        value += _weights[i - 1][j][k] * _neurons[i - 1][k];
                    }
                    _neurons[i][j] = activate(value + _biases[i][j], layer);
                }
            }
            return _neurons[_neurons.Length - 1];
        }

        /** Backpropagation **/

        //all activation functions
        private float activate(float value, int layer)
        {
            switch (_activations[layer])
            {
                case 0:
                    return sigmoid(value);
                case 1:
                    return tanh(value);
                case 2:
                    return relu(value);
                case 3:
                    return leakyrelu(value);
                default:
                    return relu(value);
            }
        }

        //all activation function derivatives
        public float activateDer(float value, int layer)
        {
            switch (_activations[layer])
            {
                case 0:
                    return sigmoidDer(value);
                case 1:
                    return tanhDer(value);
                case 2:
                    return reluDer(value);
                case 3:
                    return leakyreluDer(value);
                default:
                    return reluDer(value);
            }
        }

        //activation functions and their corrosponding derivatives
        public float sigmoid(float x)
        {
            float k = (float)Math.Exp(x);
            return k / (1.0f + k);
        }
        public float tanh(float x)
        {
            return (float)Math.Tanh(x);
        }
        public float relu(float x)
        {
            return (0 >= x) ? 0 : x;
        }
        public float leakyrelu(float x)
        {
            return (0 >= x) ? 0.01f * x : x;
        }
        public float sigmoidDer(float x)
        {
            return x * (1 - x);
        }
        public float tanhDer(float x)
        {
            return 1 - (x * x);
        }
        public float reluDer(float x)
        {
            return (0 >= x) ? 0 : 1;
        }
        public float leakyreluDer(float x)
        {
            return (0 >= x) ? 0.01f : 1;
        }

        public void BackPropagate(float[] inputs, float[] expected)
        {
            //runs feed forward to ensure neurons are populated correctly
            float[] output = FeedForward(inputs);

            Cost = 0;

            for (int i = 0; i < output.Length; i++) Cost += (float)Math.Pow(output[i] - expected[i], 2); //calculated cost of network
            Cost = Cost / 2; //this value is not used in calculions, rather used to identify the performance of the network

            float[][] gamma;


            List<float[]> gammaList = new List<float[]>();
            for (int i = 0; i < _layers.Length; i++)
            {
                gammaList.Add(new float[_layers[i]]);
            }
            gamma = gammaList.ToArray();//gamma initialization

            int layer = _layers.Length - 2;
            for (int i = 0; i < output.Length; i++) gamma[_layers.Length - 1][i] = (output[i] - expected[i]) * activateDer(output[i], layer);//Gamma calculation
            for (int i = 0; i < _neurons[_layers.Length - 1].Length; i++)//calculates the w' and b' for the last layer in the network
            {
                _biases[_layers.Length - 1][i] -= gamma[_layers.Length - 1][i] * LearningRate;
                for (int j = 0; j < _neurons[_layers.Length - 2].Length; j++)
                {

                    _weights[_layers.Length - 2][i][j] -= gamma[_layers.Length - 1][i] * _neurons[_layers.Length - 2][j] * LearningRate;//*learning 
                }
            }

            for (int i = _layers.Length - 2; i > 0; i--)//runs on all hidden layers
            {
                layer = i - 1;
                for (int j = 0; j < _neurons[i].Length; j++)//outputs
                {
                    gamma[i][j] = 0;
                    for (int k = 0; k < gamma[i + 1].Length; k++)
                    {
                        gamma[i][j] = gamma[i + 1][k] * _weights[i][k][j];
                    }
                    gamma[i][j] *= activateDer(_neurons[i][j], layer);//calculate gamma
                }
                for (int j = 0; j < _neurons[i].Length; j++)//itterate over outputs of layer
                {
                    _biases[i][j] -= gamma[i][j] * LearningRate;//modify biases of network
                    for (int k = 0; k < _neurons[i - 1].Length; k++)//itterate over inputs to layer
                    {
                        _weights[i - 1][j][k] -= gamma[i][j] * _neurons[i - 1][k] * LearningRate;//modify weights of network
                    }
                }
            }
        }

        /* Genetic implementation */

        public void Mutate(int high, float val)//used as a simple mutation function for any genetic implementations.
        {
            for (int i = 0; i < _biases.Length; i++)
            {
                for (int j = 0; j < _biases[i].Length; j++)
                {
                    _biases[i][j] = (RandomRange(0f, high) <= 2) ? _biases[i][j] += RandomRange(-val, val) : _biases[i][j];
                }
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        _weights[i][j][k] = (RandomRange(0f, high) <= 2) ? _weights[i][j][k] += RandomRange(-val, val) : _weights[i][j][k];
                    }
                }
            }
        }

        public int CompareTo(BasicNeuralNetwork other) //Comparing For Genetic implementations. Used for sorting based on the fitness of the network
        {
            if (other == null) return 1;

            if (Fitness > other.Fitness)
                return 1;
            else if (Fitness < other.Fitness)
                return -1;
            else
                return 0;
        }

        public BasicNeuralNetwork copy(BasicNeuralNetwork nn) //For creating a deep copy, to ensure arrays are serialzed.
        {
            for (int i = 0; i < _biases.Length; i++)
            {
                for (int j = 0; j < _biases[i].Length; j++)
                {
                    nn._biases[i][j] = _biases[i][j];
                }
            }
            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        nn._weights[i][j][k] = _weights[i][j][k];
                    }
                }
            }
            return nn;
        }

        //save and load functions
        public void Load(string path)//this loads the biases and weights from within a file into the neural network.
        {
            TextReader tr = new StreamReader(path);
            int NumberOfLines = (int)new FileInfo(path).Length;
            string[] ListLines = new string[NumberOfLines];
            int index = 1;
            for (int i = 1; i < NumberOfLines; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
            tr.Close();
            if (new FileInfo(path).Length > 0)
            {
                for (int i = 0; i < _biases.Length; i++)
                {
                    for (int j = 0; j < _biases[i].Length; j++)
                    {
                        _biases[i][j] = float.Parse(ListLines[index]);
                        index++;
                    }
                }

                for (int i = 0; i < _weights.Length; i++)
                {
                    for (int j = 0; j < _weights[i].Length; j++)
                    {
                        for (int k = 0; k < _weights[i][j].Length; k++)
                        {
                            _weights[i][j][k] = float.Parse(ListLines[index]); ;
                            index++;
                        }
                    }
                }
            }
        }
        public void Save(string path) //this is used for saving the biases and weights within the network to a file.
        {
            File.Create(path).Close();
            StreamWriter writer = new StreamWriter(path, true);

            for (int i = 0; i < _biases.Length; i++)
            {
                for (int j = 0; j < _biases[i].Length; j++)
                {
                    writer.WriteLine(_biases[i][j]);
                }
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        writer.WriteLine(_weights[i][j][k]);
                    }
                }
            }
            writer.Close();
        }

        private float RandomRange(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

       
    }
}
