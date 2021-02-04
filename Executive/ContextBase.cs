using AIkailo.Core.Model;
using System.Threading;

namespace AIkailo.Executive
{
    internal class ContextBase
    {

        private Thread _thread;

        private volatile bool _isRunning;
        private volatile bool _isStopping;

        internal ContextBase()
        {
            _thread = new Thread(Run);
        }

        internal void Start()
        {
            if (_thread.ThreadState != ThreadState.Running)
            {
                _thread.Start();
            }
        }

        internal void Stop()
        {

            _isStopping = true;
            _thread.Interrupt();
        }

        private void Run()
        {
            if (_isRunning) { return; }

            _isRunning = true;

            while (!_isStopping)
            {
                try
                {
                    Next();
                }
                catch (ThreadInterruptedException ex)
                {
                    _ = ex;
                }
            }

            _isRunning = false;
            _isStopping = false;
        }

        internal virtual void Next() { }
    }
}