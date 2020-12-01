using System;

namespace AIkailo.Core.Model
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
