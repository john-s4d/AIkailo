using AIkailo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    internal class Trainer
    {
        private IDataProvider _dataProvider;

        public Trainer(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        internal Task Train(List<Node> input, List<Node> output)
        {
            // Fit a process model

            // One-To-One
            if (input.Count == 1 && output.Count == 1)
            {
                // TODO: Merge a one-to-one process node                
            }

            throw new NotImplementedException();
        }
    }
}
