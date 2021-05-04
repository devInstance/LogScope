using DevInstance.LogScope.Formatters;
using System;

namespace DevInstance.LogScope.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ======== Default options ========");
            var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.TRACE);

            new TestClass(manager).MethodA();

            Console.WriteLine(" ======== With Timestamp and Thread options ========");
            manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.TRACE, new DefaultFormattersOptions { ShowTimestamp = true, ShowThreadNumber = true });

            new TestClass(manager).MethodA();

            var log = manager.CreateLogger("log test");
            log.E("Logging an error");
            log.W("Logging a warning");
            log.I("Logging an information");
            log.D("Logging a debug line");
            log.T("Logging a trace line");

            Console.ReadKey();
        }
    }
}
