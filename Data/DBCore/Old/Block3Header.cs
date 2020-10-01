using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data.DBCore
{
    public class Block3Header
    {
        public bool IsDeleted { get; private set; }
        public int HeaderLength { get; private set; }
        public int ContentLength { get; private set; }
    }
}
