using AIkailo.Common;
using System.Timers;

namespace AIkailo.Executive
{
    internal class TimeProvider : ITimeProvider
    {
        
        public event TickEventHandler Tick;

        internal const int INTERVAL = 100;

        private Timer _timer = new Timer();

        private RandomProvider _random = new RandomProvider();

        private long tickCount = 0;

        internal TimeProvider()
        {
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = INTERVAL;
            _timer.AutoReset = true;            
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double randomDouble = _random.NextDouble();
            bool randomBool = randomDouble >= 0.5;

            TickEventArgs tickArgs = new TickEventArgs(e.SignalTime, randomDouble, randomBool, tickCount++);
            Tick?.Invoke(tickArgs);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
