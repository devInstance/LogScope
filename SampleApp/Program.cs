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

            Console.WriteLine(" ======== With Timestamp, Thread and SHow Id options ========");
            manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.TRACE, new DefaultFormattersOptions { ShowTimestamp = true, ShowThreadNumber = true, ShowId = true });
            new TestClass(manager).MethodA();

            var log = manager.CreateLogger("log test");
            LoggingTest(log);

            Console.WriteLine(" ======== With Override ========");
            manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.INFO, new DefaultFormattersOptions { ShowTimestamp = true, ShowThreadNumber = true, ShowId = true });
            new TestClass(manager).MethodA();

            Console.WriteLine(" ---Default level");
            log = manager.CreateLogger("log test");
            LoggingTest(log);

            Console.WriteLine(" ---Override level");
            log = manager.CreateLogger("log test", LogLevel.TRACE);
            LoggingTest(log);

            Console.ReadKey();
        }

        private static void LoggingTest(IScopeLog log)
        {
            log.E("Logging an error");
            log.W("Logging a warning");
            log.I("Logging an information");
            log.D("Logging a debug line");
            log.T("Logging a trace line");
        }
    }
}
