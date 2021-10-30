using AIkailo.Common;
using AIkailo.Neural.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Executive
{
    public class Trainer
    {
        private INeuronProvider _nodeProvider;

        public Trainer(INeuronProvider nodeProvider)
        {
            _nodeProvider = nodeProvider;
        }        
    }
}
