# LogScope.NET Standard: Logging, tracing and profiling

LogScope.NET is a lightweight logging framework designed for tracing, profiling, and logging critical parts of your code. It can seamlessly integrate with popular logging libraries like Log4Net and NLog.

The framework revolves around the concept of a "scope," which can represent a method or a specific part of it. The implementation is based on the IDisposable interface, where calling Dispose method marks the end of the scope.

Additionally, it offers more streamlined coding and shorter lines of code compared to the conventional logging APIs.

Here's an example that illustrates this approach:
```csharp
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
21-01-29 23:07:24--> TestClass:MethodA
21-01-29 23:07:24    TestClass:MethodA:Wait for 200 msec
21-01-29 23:07:25    TestClass:MethodA:Done.
21-01-29 23:07:25--> TestClass:MethodA:a-scope
21-01-29 23:07:25    TestClass:MethodA:a-scope:Inside of a scope
21-01-29 23:07:25<-- TestClass:MethodA:a-scope, time:2.1252 msec
21-01-29 23:07:25--> TestClass:MethodB
21-01-29 23:07:25    TestClass:MethodB:Inside of method B scope
21-01-29 23:07:25<-- TestClass:MethodB, time:212.748 msec
21-01-29 23:07:25<-- TestClass:MethodA, time:480.5525 msec
```

## Setup

LogScope package is available on NuGet (https://www.nuget.org/packages/DevInstance.LogScope/). The default implementation is based on simple Console.Log calls. There is an extension available for the standard Microsoft logger: https://www.nuget.org/packages/DevInstance.LogScope.Extensions.MicrosoftLogger/

So, add the package reference to your project:

**Power shell:**

- .Net Standard 2.0: ```dotnet add package DevInstance.LogScope```
- .Net 6+: ```dotnet add package DevInstance.LogScope.NET```
- Mocrosoft Loggering extension (.Net 6+): ```DevInstance.LogScope.Extensions.MicrosoftLogger``` 

**Package manager**:

- .Net Standard 2.0: ```Install-Package DevInstance.LogScope```
- .Net 6+: ```Install-PackageDevInstance.LogScope.NET```
- Mocrosoft Loggering extension (.Net 6+): ```Install-Package DevInstance.LogScope.Extensions.MicrosoftLogger```

## Initialization

Initialization differs based on the type of the application and logging provider. If application uses dependency injection just call the AddConsoleScopeLogging extension method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddConsoleScopeLogging(LogLevel.DEBUG);
    //...
}
```

For console application if can be instantiated by using DefaultScopeLogFactory directly:

var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);

For MicrosoftLogger extension, call:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddMicrosoftScopeLogging(new DefaultFormattersOptions { ShowTimestamp = true, ShowThreadNumber = true });
    //...
}
```

See more information about API: http://logscope.devinstance.net/

