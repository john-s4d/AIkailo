using AIkailo.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    internal class OneToOnePassThrough //: ProcessModel
    {
        //public override bool CanBackPropagate { get; protected set; } = false;
        public bool CanMutate { get; protected set; } = false;
        /*
        public override void FeedForward()
        {
            for (int i = 0; i < Input.Count; i++)
            {
                Output[i].Value = Input[i].Value;
            }
        }*/

        internal void Initialize()
        {
            throw new NotImplementedException();
        }

        internal void BackPropagate()
        {
            throw new NotImplementedException();
        }

        internal void Mutate()
        {
            throw new NotImplementedException();
        }
    }
}
