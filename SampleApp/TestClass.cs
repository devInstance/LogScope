using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DevInstance.LogScope.SampleApp
{
    class TestClass
    {
        IScopeLog classScope;

        public TestClass(IScopeManager manager)
        {
            classScope = manager.CreateLogger(this);
        }

        public void MethodA()
        {
            using (var methodScope = classScope.DebugScope())
            {
                methodScope.D("Wait for 200 msec");
                Thread.Sleep(200);
                methodScope.D("Done.");
                using (var aScope = methodScope.DebugScope("a-scope"))
                {
                    aScope.D("Inside of a scope");
                }
                MethodB();
            }
        }

        private void MethodB()
        {
            using (var methodScope = classScope.DebugScope())
            {
                methodScope.D("Inside of method B scope");
                Thread.Sleep(200);
            }
        }
    }
}
