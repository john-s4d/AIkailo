using System;

namespace AIkailo.Common
{
    public interface IAIkailoService //: IDisposable
    {
        string Name { get; }
        AkailoServiceState State { get; }
        void Start();
        void Stop();
    }

    public enum AkailoServiceState
    {
        STARTED,
        STOPPED
    }
}
