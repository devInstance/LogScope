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

LogScope packages are available on NuGet:
- [DevInstance.LogScope](https://www.nuget.org/packages/DevInstance.LogScope/) - Core library (.NET 10)
- [DevInstance.LogScope.NET](https://www.nuget.org/packages/DevInstance.LogScope.NET/) - .NET 10+ with DI support
- [DevInstance.LogScope.Extensions.MicrosoftLogger](https://www.nuget.org/packages/DevInstance.LogScope.Extensions.MicrosoftLogger/) - Microsoft.Extensions.Logging integration

Add the package reference to your project:

**CLI:**

```bash
# Core library (.NET 10)
dotnet add package DevInstance.LogScope

# .NET 10+ with dependency injection
dotnet add package DevInstance.LogScope.NET

# Microsoft Logging extension
dotnet add package DevInstance.LogScope.Extensions.MicrosoftLogger
```

**Package Manager:**

```powershell
# Core library (.NET 10)
Install-Package DevInstance.LogScope

# .NET 10+ with dependency injection
Install-Package DevInstance.LogScope.NET

# Microsoft Logging extension
Install-Package DevInstance.LogScope.Extensions.MicrosoftLogger
```

## Initialization

### With Dependency Injection

If your application uses dependency injection, call the `AddConsoleScopeLogging` extension method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddConsoleScopeLogging(LogLevel.DEBUG);
}
```

### Console Application (without DI)

For console applications, instantiate using `DefaultScopeLogFactory` directly:

```csharp
var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);
```

### With Microsoft.Extensions.Logging

For integration with Microsoft's logging framework:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMicrosoftScopeLogging(new DefaultFormattersOptions
    {
        ShowTimestamp = true,
        ShowThreadNumber = true
    });
}
```

## Log Levels

LogScope supports the following log levels:

| Level | Description |
|-------|-------------|
| `NOLOG` | Disable logging |
| `ERROR` | Error messages only |
| `WARNING` | Warnings and errors |
| `INFO` | Informational messages |
| `DEBUG` | Debug messages (verbose) |

## Documentation

See more information about the API: http://logscope.devinstance.net/

