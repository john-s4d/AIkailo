using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AIkailo.Common
{
    public interface ITimeProvider
    {   
        event TickEventHandler Tick;
        
        void Start();
        void Stop();
    }

    public delegate void TickEventHandler(TickEventArgs e);

    public class TickEventArgs
    {
        public DateTime TickTime { get; }
        public long TickNumber { get; }
        public double RandomDouble { get; }
        public bool RandomBool { get; }

        public TickEventArgs(DateTime tickTime, double randomDouble, bool randomBool, long tickNumber)
        {
            TickTime = tickTime;
            TickNumber = tickNumber;
            RandomDouble = randomDouble;
            RandomBool = randomBool;
        }
    }
}
