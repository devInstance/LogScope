# LogScope.NET: Logging, tracing and profiling

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
- [DevInstance.LogScope.Extensions.SerilogLogger](https://www.nuget.org/packages/DevInstance.LogScope.Extensions.SerilogLogger/) - Serilog integration

Add the package reference to your project:

**CLI:**

```bash
# Core library (.NET 10)
dotnet add package DevInstance.LogScope

# .NET 10+ with dependency injection
dotnet add package DevInstance.LogScope.NET

# Microsoft Logging extension
dotnet add package DevInstance.LogScope.Extensions.MicrosoftLogger

# Serilog extension
dotnet add package DevInstance.LogScope.Extensions.SerilogLogger
```

**Package Manager:**

```powershell
# Core library (.NET 10)
Install-Package DevInstance.LogScope

# .NET 10+ with dependency injection
Install-Package DevInstance.LogScope.NET

# Microsoft Logging extension
Install-Package DevInstance.LogScope.Extensions.MicrosoftLogger

# Serilog extension
Install-Package DevInstance.LogScope.Extensions.SerilogLogger
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
    services.AddMicrosoftScopeLogging(LogLevel.DEBUG, "MyApp");
}
```

### With Serilog

For integration with Serilog, using the global `Log.Logger`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSerilogScopeLogging(LogLevel.DEBUG);
}
```

Or with a custom Serilog logger instance:

```csharp
var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateLogger();

services.AddSerilogScopeLogging(serilogLogger, LogLevel.DEBUG);
```

For console applications without DI, use the factory directly:

```csharp
var manager = SerilogLogProviderFactory.CreateManager(
    LogLevel.DEBUG,
    Log.Logger,
    new DefaultFormattersOptions { ShowTimestamp = true });
```

## Message Templates

LogScope supports Serilog-style structured message templates. Placeholders in the template string are replaced with argument values in order:

```csharp
log.I("Processed {Count} items in {Elapsed} ms", count, elapsed);
// Output: Processed 5 items in 120 ms
```

### Format Specifiers

Apply .NET format strings using the `{Name:format}` syntax:

```csharp
log.I("Elapsed: {Elapsed:000} ms", 34);
// Output: Elapsed: 034 ms

log.I("Date: {Date:yyyy-MM-dd}", DateTime.Now);
// Output: Date: 2026-02-26
```

### Destructuring

Use the `@` operator to destructure objects into their properties or enumerate collections:

```csharp
log.I("Position: {@Pos}", new { Latitude = 25, Longitude = 134 });
// Output: Position: { Latitude: 25, Longitude: 134 }

log.I("Items: {@Items}", new List<int> { 1, 2, 3 });
// Output: Items: [1, 2, 3]
```

### Escaped Braces

Use `{{` and `}}` to output literal braces:

```csharp
log.I("Value is {{not a placeholder}}");
// Output: Value is {not a placeholder}
```

Template overloads are available for all log levels: `T()`, `D()`, `I()`, `W()`, and `E()`.

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

See more information about the API: https://devinstance.net/engineering/logscope

