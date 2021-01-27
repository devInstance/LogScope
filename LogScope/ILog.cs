﻿using System;

namespace DevInstance.LogScope
{
    public enum LogLevel
    {
        NOLOG = 0,
        ERROR = 2,
        INFO = 4,
        DEBUG = 8,
        DEBUG_EXTRA = 10
    }

    public interface ILog : IDisposable
    {
        void Line(LogLevel level, string message);
        ILog Scope(LogLevel level, string scope);
    }
}
