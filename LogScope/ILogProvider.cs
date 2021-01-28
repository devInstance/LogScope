using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    public interface ILogProvider
    {
        void WriteLine(string line);
    }
}
