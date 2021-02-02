# LogScope

.NET Standard 2.0: Logging, tracing and profiling

LogScope implements simple logging framework for tracing, profiling and logging methods and critical parts of the code. It can be integrated with popular logging library (such as Log4Net, NLog, etc). The whole idea is around using a “scope”. Scope can be method or a specific part of it. The implementation is based on IDisposable where calling Dispose ends the scope. The following example demonstrate this approach:

```private void Foo()
   {
       using (var methodScope = classScope.DebugScope())
       {
           methodScope.D("Inside of method Foo scope");
           Thread.Sleep(200);
       }
   }
```
Result: 
```
--> begin of Foo
   Foo: Inside of method Foo scope
<-- end of Foo, time:3 msec
```
It can be useful for the apps to trace the performance or async code calls. 
```
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
```
Result:
```
21-01-29 23:07:24--> begin of TestClass:MethodA
21-01-29 23:07:24       TestClass:MethodA:Wait for 200 msec
21-01-29 23:07:25       TestClass:MethodA:Done.
21-01-29 23:07:25--> begin of TestClass:MethodA:a-scope
21-01-29 23:07:25       TestClass:MethodA:a-scope:Inside of a scope
21-01-29 23:07:25<-- end of TestClass:MethodA:a-scope, time:2.1252 msec
21-01-29 23:07:25--> begin of TestClass:MethodB
21-01-29 23:07:25       TestClass:MethodB:Inside of method B scope
21-01-29 23:07:25<-- end of TestClass:MethodB, time:212.748 msec
21-01-29 23:07:25<-- end of TestClass:MethodA, time:480.5525 msec
```

See more https://devinstance.net/blog/dotnet-standard-logging-and-profiling
