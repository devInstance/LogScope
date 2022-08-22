using DevInstance.LogScope.Formatters;
using DevInstance.LogScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace InternalTestApp
{
    class EmptyScope : IDisposable
    {
        public void Dispose()
        {
        }
    }

    struct LContext
    {
        public ILogProvider Provider { get; }
        public IScopeManager Manager { get; }
        public IScopeFormatter Formatter { get; }
    }

    class TestScope : IDisposable
    {
        DateTime timeStart;
        string Name { get; set; }
        public ILogProvider Provider { get; }
        public IScopeManager Manager { get; }
        public IScopeFormatter Formatter { get; }
        public LogLevel ScopeLevel { get; } = LogLevel.WARNING;

        private string scopeId;

        public TestScope()
        {
        }

        public TestScope(string scope) => scopeId = scope;

        public TestScope(string scope, string name, ILogProvider provider, IScopeManager manager, IScopeFormatter scopeFormatter)
        {
            //scopeId = scope;
            //Name = name;
            //Provider = provider;
            //Manager = manager;
            //Formatter = scopeFormatter;
        }

        public TestScope(string scope, string name, LContext ctx)
        {
            scopeId = scope;
            Provider = ctx.Provider;
            Manager = ctx.Manager;
            Formatter = ctx.Formatter;
        }

        public void Dispose()
        {
        }
    }

    internal class PerfComparisonTest
    {
        public void Test(IScopeManager manager)
        {
            var scope = manager.CreateLogger("MainLogger");
            //long maxCycles = 100000000;
            long maxCycles = 1000000;

            scope.I(" ======== Warm up ========");
            TestBasicScope(scope, maxCycles, 0);

            scope.I(" ======== Go ========");

            //TestBasicScope(scope, maxCycles, 3);
            //TestBasicScope(scope, maxCycles, 2);
            //TestBasicScope(scope, maxCycles, 1);
            //TestBasicScope(scope, maxCycles, 0);

            //TestBasicScope(scope, maxCycles, 3);
            //TestBasicScope(scope, maxCycles, 2);
            //TestBasicScope(scope, maxCycles, 1);
            //TestBasicScope(scope, maxCycles, 0);

            TestDebugScope(scope, maxCycles);
            TestDebugScope(scope, maxCycles);
            TestDebugScope(scope, maxCycles);

            TestEmptyLoop(scope, maxCycles);
            TestEmptyLoop(scope, maxCycles);

        }

        private static void TestEmptyLoop(IScopeLog scope, long mm)
        {
            long test = 1;
            using (var log = scope.InfoScope("Empty loop"))
            {
                for (var i = 0; i < mm; i++)
                {
                    test++;
                }
            }
        }

        private static void TestDebugScope(IScopeLog scope, long mm)
        {
            long test = 1;
            using (var log = scope.InfoScope("Debug scope loop"))
            {
                for (var i = 0; i < mm; i++)
                {
                    using (var logtest = scope.DebugScope())
                    {
                        //logtest.T("test");
                        test++;
                    }
                }
            }
        }

        private static void TestBasicScope(IScopeLog scope, long mm, int t)
        {
            long test = 1;

            if(t == 0)
            {
                using (var log = scope.InfoScope("Empty scope loop"))
                {
                    for (var i = 0; i < mm; i++)
                    {
                        using (var logtest = new EmptyScope())
                        {
                            test++;
                        }
                    }
                }
            }
            else if(t == 1)
            {
                using (var log = scope.InfoScope("Basic scope loop"))
                {
                    for (var i = 0; i < mm; i++)
                    {
                        using (var logtest = new TestScope("test"))
                        {
                            test++;
                        }
                    }
                }
            }
            else if (t == 2)
            {
                using (var log = scope.InfoScope("Basic param loop"))
                {
                    for (var i = 0; i < mm; i++)
                    {
                        using (var logtest = new TestScope("test", "test", null, null, null))
                        {
                            test++;
                        }
                    }
                }
            }
            else if (t == 3)
            {
                var context = new LContext();
                using (var log = scope.InfoScope("CTX param loop"))
                {
                    for (var i = 0; i < mm; i++)
                    {
                        using (var logtest = new TestScope("test", "test", context))
                        {
                            test++;
                        }
                    }
                }
            }
        }
    }
}
