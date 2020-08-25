﻿using System;

namespace AIkailo.Model
{
    public interface IAIkailoService //: IDisposable
    {
        string Name { get; }
        IAkailoServiceState State { get; }
        void Start();
        void Stop();
    }

    public enum IAkailoServiceState
    {
        STARTED,
        STOPPED
    }
}
