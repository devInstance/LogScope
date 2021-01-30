using DevInstance.LogScope.Providers.Console;
using System;

namespace DevInstance.LogScope.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = ScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG_EXTRA, true);

            var test = new TestClass(manager);
            test.MethodA();

            Console.ReadKey();
        }
    }
}
