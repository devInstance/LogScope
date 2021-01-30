using DevInstance.LogScope.Providers.Console;
using System;

namespace DevInstance.LogScope.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = ScopeLogFactory.Create(LogLevel.DEBUG_EXTRA, new ConsoleLogProvider());
            var looger = manager.CreateLogger("SampleApp");
            looger.D("Hello World!");
            using (var log = looger.DebugExScope())
            {
                log.D("Hello from the scope A");
                using (var sublog = log.DebugExScope("Sub-scope"))
                {
                    sublog.D("Hello from the scope B");
                }
            }

            Console.ReadKey();
        }
    }
}
