﻿using DevInstance.LogScope.Formaters;
using System;

namespace DevInstance.LogScope.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ======== Default options ========");
            var manager = ScopeLogFactory.CreateConsoleLogger(LogLevel.TRACE);

            new TestClass(manager).MethodA();

            Console.WriteLine(" ======== With Timestamp and Thread options ========");
            manager = ScopeLogFactory.CreateConsoleLogger(LogLevel.TRACE, new DefaultFormaterOptions { ShowTimestamp = true, ShowThreadNumber = true });

            new TestClass(manager).MethodA();

            Console.ReadKey();
        }
    }
}
