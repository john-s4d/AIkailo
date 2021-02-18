using AIkailo.Core.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    internal abstract class ProcessModel : Node
    {
        internal abstract void Initialize();
        
        public override abstract void FeedForward();
        internal abstract void BackPropagate();
        internal abstract void Mutate();
        public abstract bool CanBackPropagate { get; protected set; }
        public abstract bool CanMutate { get; protected set;  }

        internal List<Node> Input { get; set; }
        internal List<Node> Output { get; set; }        

        internal static ProcessModel Create(string modelName)
        {
            switch (modelName)
            {
                case "OneToOnePassThrough":
                    return new OneToOnePassThrough();
                default:
                    return new BasicNeuralNetwork(); ;
            }
        }
    }
}
