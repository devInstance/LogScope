# LogScope.NET Usage Guide

A comprehensive guide to using LogScope.NET for tracing, profiling, and logging in .NET applications.

## Table of Contents

- [Installation & Setup](#installation--setup)
- [Core Concepts](#core-concepts)
- [Basic Usage](#basic-usage)
- [Log Levels](#log-levels)
- [Scope Methods](#scope-methods)
- [Configuration Options](#configuration-options)
- [Integration Examples](#integration-examples)
- [Message Templates](#message-templates)
- [Output Format](#output-format)
- [Complete Examples](#complete-examples)

---

## Installation & Setup

### Requirements

- .NET 10 SDK or later

### Package Installation

Choose the package that fits your needs:

```bash
# Core library
dotnet add package DevInstance.LogScope

# With dependency injection support
dotnet add package DevInstance.LogScope.NET

# Microsoft.Extensions.Logging integration
dotnet add package DevInstance.LogScope.Extensions.MicrosoftLogger

# Serilog integration
dotnet add package DevInstance.LogScope.Extensions.SerilogLogger
```

### Quick Start

**With Dependency Injection:**

```csharp
using DevInstance.LogScope;

var builder = WebApplication.CreateBuilder(args);

// Add scope logging with console output
builder.Services.AddConsoleScopeLogging(LogLevel.DEBUG);

var app = builder.Build();
```

**Console Application (Non-DI):**

```csharp
using DevInstance.LogScope;

var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);
var log = manager.CreateLogger(this);
```

**With Microsoft.Extensions.Logging:**

```csharp
using DevInstance.LogScope.Extensions.MicrosoftLogger;

public void ConfigureServices(IServiceCollection services)
{
    services.AddMicrosoftScopeLogging(LogLevel.DEBUG, "MyApp");
}
```

**With Serilog:**

```csharp
using DevInstance.LogScope.Extensions.SerilogLogger;

// Using the global Log.Logger
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

---

## Core Concepts

### What is a Scope?

A scope represents a method or code section that you want to trace. LogScope uses the `IDisposable` pattern where:

- Creating a scope marks the **entry point**
- `Dispose()` (end of `using` block) marks the **exit point**
- Execution time is automatically measured between entry and exit

### Why Use Scopes?

LogScope provides more streamlined coding compared to conventional logging APIs:

**Traditional Logging:**
```csharp
public void ProcessOrder(Order order)
{
    _logger.LogDebug("ProcessOrder started");
    var stopwatch = Stopwatch.StartNew();

    try
    {
        // Process order...
        _logger.LogDebug("Processing order {OrderId}", order.Id);
    }
    finally
    {
        stopwatch.Stop();
        _logger.LogDebug("ProcessOrder completed in {Elapsed}ms", stopwatch.ElapsedMilliseconds);
    }
}
```

**With LogScope:**
```csharp
public void ProcessOrder(Order order)
{
    using var scope = log.DebugScope();

    // Process order...
    scope.D($"Processing order {order.Id}");
}
```

---

## Basic Usage

### Creating a Logger

Inject `IScopeManager` and create a logger in your class:

```csharp
using DevInstance.LogScope;

public class OrderService
{
    private readonly IScopeLog log;

    public OrderService(IScopeManager logManager)
    {
        log = logManager.CreateLogger(this);
    }
}
```

### Using Scopes

Wrap code sections in scopes using the `using` statement:

```csharp
public void ProcessOrder(Order order)
{
    using var scope = log.DebugScope();

    // Your code here...
    scope.D("Order processed successfully");
}
```

### Nested Scopes

Scopes can be nested within a method by creating a new scope from an existing one. Pass a custom name to `DebugScope()` to label the nested scope:

```csharp
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

### Passing Scopes to Methods

You can pass a scope to another method to create a child scope that appears nested under the caller. This is useful for helper methods that should be traced as part of the parent operation:

```csharp
public void ProcessOrder(Order order)
{
    using (var scope = classScope.DebugScope())
    {
        scope.I("Processing order {OrderId}", order.Id);
        ValidateOrder(scope, order);
        CalculateTotals(scope, order);
        scope.I("Order processed successfully");
    }
}

private void ValidateOrder(IScopeLog parentScope, Order order)
{
    using (var scope = parentScope.DebugScope("ValidateOrder"))
    {
        scope.D("Checking required fields");
        // Validation logic...
        scope.D("Validation passed");
    }
}

private void CalculateTotals(IScopeLog parentScope, Order order)
{
    using (var scope = parentScope.DebugScope("CalculateTotals"))
    {
        scope.D("Calculating subtotal for {Count} items", order.Items.Count);
        order.Subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        order.Total = order.Subtotal + order.Tax;
        scope.D("Total: {Total:F2}", order.Total);
    }
}
```

Result:
```
23:07:24--> OrderService:ProcessOrder
23:07:24    OrderService:ProcessOrder:Processing order ORD-123
23:07:24--> OrderService:ProcessOrder:ValidateOrder
23:07:24    OrderService:ProcessOrder:ValidateOrder:Checking required fields
23:07:24    OrderService:ProcessOrder:ValidateOrder:Validation passed
23:07:24<-- OrderService:ProcessOrder:ValidateOrder, time:1.234 msec
23:07:24--> OrderService:ProcessOrder:CalculateTotals
23:07:24    OrderService:ProcessOrder:CalculateTotals:Calculating subtotal for 3 items
23:07:24    OrderService:ProcessOrder:CalculateTotals:Total: 59.97
23:07:24<-- OrderService:ProcessOrder:CalculateTotals, time:0.567 msec
23:07:24    OrderService:ProcessOrder:Order processed successfully
23:07:24<-- OrderService:ProcessOrder, time:5.678 msec
```

Notice how `ValidateOrder` and `CalculateTotals` appear nested under `ProcessOrder` in the output, since their scopes were created from the parent scope rather than from the class-level logger.

---

## Log Levels

LogScope supports five log levels, from least to most verbose:

| Level | Description | Use Case |
|-------|-------------|----------|
| `NOLOG` | Logging disabled | Production with no logging |
| `ERROR` | Error messages only | Production environments |
| `WARNING` | Warnings and errors | Production with diagnostics |
| `INFO` | Informational messages | General monitoring |
| `DEBUG` | All messages | Development and debugging |

### Setting Log Level

```csharp
// At startup
services.AddConsoleScopeLogging(LogLevel.DEBUG);

// Or for console apps
var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.INFO);
```

---

## Scope Methods

### Creating Scopes

| Method | Level | Description |
|--------|-------|-------------|
| `DebugScope()` | DEBUG | Creates a debug-level scope |
| `InfoScope()` | INFO | Creates an info-level scope |
| `WarningScope()` | WARNING | Creates a warning-level scope |
| `ErrorScope()` | ERROR | Creates an error-level scope |

### Logging Within Scopes

| Method | Level | Description |
|--------|-------|-------------|
| `scope.D(message)` | DEBUG | Log debug message |
| `scope.I(message)` | INFO | Log info message |
| `scope.W(message)` | WARNING | Log warning message |
| `scope.E(message)` | ERROR | Log error message |
| `scope.E(exception)` | ERROR | Log exception |
| `scope.D(template, args)` | DEBUG | Log debug with message template |
| `scope.I(template, args)` | INFO | Log info with message template |
| `scope.W(template, args)` | WARNING | Log warning with message template |
| `scope.E(template, args)` | ERROR | Log error with message template |

### Example

```csharp
public async Task<Order> GetOrderAsync(string id)
{
    using var scope = log.DebugScope();

    scope.D($"Fetching order {id}");

    var order = await _repository.FindAsync(id);

    if (order == null)
    {
        scope.W($"Order {id} not found");
        return null;
    }

    scope.I($"Found order with {order.Items.Count} items");
    return order;
}
```

---

## Configuration Options

### DefaultFormattersOptions

Configure the output format when using Microsoft.Extensions.Logging integration:

```csharp
services.AddMicrosoftScopeLogging(new DefaultFormattersOptions
{
    ShowTimestamp = true,      // Show timestamp in output
    ShowThreadNumber = true,   // Show thread ID
    ShowClassName = true,      // Show class name
    ShowMethodName = true,     // Show method name
    IndentSize = 2             // Indentation for nested scopes
});
```

### Custom Formatters

You can create custom formatters by implementing `IScopeFormatter`:

```csharp
public class CustomFormatter : IScopeFormatter
{
    public string FormatEntry(ScopeContext context)
    {
        return $"[START] {context.ClassName}.{context.MethodName}";
    }

    public string FormatExit(ScopeContext context, TimeSpan elapsed)
    {
        return $"[END] {context.ClassName}.{context.MethodName} ({elapsed.TotalMilliseconds}ms)";
    }

    public string FormatMessage(ScopeContext context, string message, LogLevel level)
    {
        return $"[{level}] {message}";
    }
}
```

---

## Integration Examples

### ASP.NET Core Web API

```csharp
// Program.cs
using DevInstance.LogScope;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddConsoleScopeLogging(LogLevel.DEBUG);

var app = builder.Build();
app.MapControllers();
app.Run();
```

```csharp
// OrdersController.cs
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IScopeLog log;
    private readonly IOrderService _orderService;

    public OrdersController(IScopeManager logManager, IOrderService orderService)
    {
        log = logManager.CreateLogger(this);
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(string id)
    {
        using var scope = log.DebugScope();

        scope.D($"API request for order {id}");

        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
        {
            scope.W("Order not found");
            return NotFound();
        }

        return Ok(order);
    }
}
```

### Microsoft.Extensions.Logging Integration

```csharp
using DevInstance.LogScope.Extensions.MicrosoftLogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftScopeLogging(new DefaultFormattersOptions
{
    ShowTimestamp = true,
    ShowThreadNumber = true
});

// Your existing ILogger instances will work alongside LogScope
builder.Logging.AddConsole();
```

### Console Application

```csharp
using DevInstance.LogScope;

class Program
{
    static void Main(string[] args)
    {
        var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);

        var processor = new DataProcessor(manager);
        processor.ProcessData();
    }
}

class DataProcessor
{
    private readonly IScopeLog log;

    public DataProcessor(IScopeManager manager)
    {
        log = manager.CreateLogger(this);
    }

    public void ProcessData()
    {
        using var scope = log.DebugScope();

        scope.I("Starting data processing");

        for (int i = 0; i < 10; i++)
        {
            ProcessItem(i);
        }

        scope.I("Data processing complete");
    }

    private void ProcessItem(int index)
    {
        using var scope = log.DebugScope();
        scope.D($"Processing item {index}");
        Thread.Sleep(100); // Simulate work
    }
}
```

---

## Message Templates

LogScope supports Serilog-style structured message templates as an alternative to string interpolation. Placeholders in the template string are replaced with argument values in order.

### Basic Usage

```csharp
scope.I("Processed {Count} items in {Elapsed} ms", count, elapsed);
// Output: Processed 5 items in 120 ms

scope.D("User {Username} logged in from {IpAddress}", username, ip);
// Output: User alice logged in from 192.168.1.1
```

### Format Specifiers

Apply .NET format strings using the `{Name:format}` syntax. The argument must implement `IFormattable` (numbers, dates, etc.):

```csharp
scope.I("Elapsed: {Elapsed:000} ms", 34);
// Output: Elapsed: 034 ms

scope.I("Total: {Amount:F2}", 19.9m);
// Output: Total: 19.90

scope.I("Created: {Date:yyyy-MM-dd}", DateTime.Now);
// Output: Created: 2026-02-26
```

### Destructuring

Use the `@` operator to destructure objects into their public properties, or enumerate collections:

```csharp
// Objects → { Property: value, ... }
scope.I("Position: {@Pos}", new { Latitude = 25, Longitude = 134 });
// Output: Position: { Latitude: 25, Longitude: 134 }

// Collections → [item, item, ...]
scope.I("Items: {@Items}", new List<int> { 1, 2, 3 });
// Output: Items: [1, 2, 3]

// Primitives and strings pass through to ToString()
scope.I("Name: {@Name}", "Alice");
// Output: Name: Alice
```

### Escaped Braces

Use `{{` and `}}` to output literal braces:

```csharp
scope.I("Value is {{not a placeholder}}");
// Output: Value is {not a placeholder}
```

### Edge Cases

Message templates handle edge cases gracefully without throwing exceptions:

| Scenario | Behavior |
|----------|----------|
| Missing arguments | Placeholder kept as literal (e.g., `{Name}`) |
| Extra arguments | Ignored |
| Null argument | Rendered as `"null"` |
| Property getter throws | Rendered as `"<error>"` |

### Templates vs String Interpolation

Both approaches work with all log methods. Use whichever style you prefer:

```csharp
// String interpolation (existing)
scope.D($"Processing order {order.Id}");

// Message template (new)
scope.D("Processing order {OrderId}", order.Id);
```

---

## Output Format

LogScope produces hierarchical output that clearly shows execution flow:

```
[12:34:56.789] --> OrdersController.GetOrder
[12:34:56.790]     API request for order ORD-123
[12:34:56.791]     --> OrderService.GetByIdAsync
[12:34:56.792]         Fetching order from database
[12:34:56.850]         --> OrderRepository.FindAsync
[12:34:56.920]         <-- OrderRepository.FindAsync (70ms)
[12:34:56.921]         Order found with 3 items
[12:34:56.922]     <-- OrderService.GetByIdAsync (131ms)
[12:34:56.923] <-- OrdersController.GetOrder (134ms)
```

### Output Elements

| Symbol | Meaning |
|--------|---------|
| `-->` | Scope entry (method start) |
| `<--` | Scope exit (method end) with elapsed time |
| Indentation | Nesting level of scopes |
| `(XXms)` | Execution time for the scope |

---

## Complete Examples

### Full Service Implementation

```csharp
using DevInstance.LogScope;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest request);
    Task<Order> GetByIdAsync(string id);
    Task<ModelList<Order>> GetOrdersAsync(OrderQuery query);
}

[WebService]
public class OrderService : IOrderService
{
    private readonly IScopeLog log;
    private readonly IOrderRepository _repository;
    private readonly IInventoryService _inventory;

    public OrderService(
        IScopeManager logManager,
        IOrderRepository repository,
        IInventoryService inventory)
    {
        log = logManager.CreateLogger(this);
        _repository = repository;
        _inventory = inventory;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        using var scope = log.DebugScope();

        scope.I($"Creating order for customer {request.CustomerId}");

        // Validate request
        ValidateRequest(request);

        // Check inventory
        await CheckInventoryAsync(request.Items);

        // Create order
        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = request.CustomerId,
            Items = request.Items,
            CreatedAt = DateTime.UtcNow
        };

        // Calculate totals
        CalculateTotals(order);

        // Save to database
        await _repository.CreateAsync(order);

        scope.I($"Order {order.Id} created successfully");
        return order;
    }

    public async Task<Order> GetByIdAsync(string id)
    {
        using var scope = log.DebugScope();

        scope.D($"Fetching order {id}");

        var order = await _repository.FindAsync(id);

        if (order == null)
        {
            scope.W($"Order {id} not found");
        }

        return order;
    }

    public async Task<ModelList<Order>> GetOrdersAsync(OrderQuery query)
    {
        using var scope = log.DebugScope();

        scope.D($"Fetching orders: Page={query.Page}, PageSize={query.PageSize}");

        return await _repository.GetPagedAsync(query);
    }

    private void ValidateRequest(CreateOrderRequest request)
    {
        using var scope = log.DebugScope();

        if (string.IsNullOrEmpty(request.CustomerId))
        {
            scope.E("Customer ID is required");
            throw new BadRequestException("Customer ID is required");
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            scope.E("Order must contain at least one item");
            throw new BadRequestException("Order must contain at least one item");
        }

        scope.D("Request validation passed");
    }

    private async Task CheckInventoryAsync(List<OrderItem> items)
    {
        using var scope = log.DebugScope();

        foreach (var item in items)
        {
            scope.D($"Checking inventory for product {item.ProductId}");

            var available = await _inventory.CheckAvailabilityAsync(
                item.ProductId,
                item.Quantity);

            if (!available)
            {
                scope.E($"Insufficient inventory for product {item.ProductId}");
                throw new BadRequestException(
                    $"Insufficient inventory for product {item.ProductId}");
            }
        }

        scope.I("All items available in inventory");
    }

    private void CalculateTotals(Order order)
    {
        using var scope = log.DebugScope();

        order.Subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        order.Tax = order.Subtotal * 0.1m;
        order.Total = order.Subtotal + order.Tax;

        scope.D($"Order total: {order.Total:C}");
    }
}
```

---

## API Reference

### Core Interfaces

| Interface | Description |
|-----------|-------------|
| `IScopeManager` | Factory for creating loggers |
| `IScopeLog` | Logger instance for a class |
| `ILogScope` | Active scope with logging methods |
| `IScopeFormatter` | Custom output formatting |

### Factory Methods

| Method | Description |
|--------|-------------|
| `DefaultScopeLogFactory.CreateConsoleLogger(level)` | Create console logger |
| `services.AddConsoleScopeLogging(level)` | Add DI console logging |
| `services.AddMicrosoftScopeLogging(options)` | Add Microsoft.Extensions.Logging integration |

### IScopeManager Methods

| Method | Description |
|--------|-------------|
| `CreateLogger(object instance)` | Create logger for class instance |
| `CreateLogger<T>()` | Create logger for type T |
| `CreateLogger(string name)` | Create logger with custom name |

### IScopeLog Methods

| Method | Description |
|--------|-------------|
| `DebugScope()` | Create DEBUG level scope |
| `InfoScope()` | Create INFO level scope |
| `WarningScope()` | Create WARNING level scope |
| `ErrorScope()` | Create ERROR level scope |

### ILogScope Methods

| Method | Description |
|--------|-------------|
| `D(string message)` | Log DEBUG message |
| `I(string message)` | Log INFO message |
| `W(string message)` | Log WARNING message |
| `E(string message)` | Log ERROR message |
| `E(Exception ex)` | Log exception |
| `D(string template, params object[] args)` | Log DEBUG with message template |
| `I(string template, params object[] args)` | Log INFO with message template |
| `W(string template, params object[] args)` | Log WARNING with message template |
| `E(string template, params object[] args)` | Log ERROR with message template |
| `T(string template, params object[] args)` | Log TRACE with message template |
