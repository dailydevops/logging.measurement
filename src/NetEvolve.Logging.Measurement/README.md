# NetEvolve.Logging.Measurement

A lightweight extension library for [Microsoft.Extensions.Logging](https://learn.microsoft.com/aspnet/core/fundamentals/logging/) that provides simple and efficient time measurement capabilities for code blocks.

> **Note:** This library is designed for quick performance insights and identifying potential bottlenecks. For comprehensive performance analysis, consider using professional profiling tools.

## Installation

```bash
dotnet add package NetEvolve.Logging.Measurement
```

## Basic Usage

The library provides the `StartMeasurement` extension method for `ILogger` instances. Measurements are automatically logged when the returned scope is disposed.

```csharp
using Microsoft.Extensions.Logging;
using NetEvolve.Logging.Measurement;

public sealed class DataProcessor
{
    private readonly ILogger<DataProcessor> _logger;

    public DataProcessor(ILogger<DataProcessor> logger)
    {
        _logger = logger;
    }

    public void ProcessData()
    {
        using (_logger.StartMeasurement("Data Loading"))
        {
            // Load data from database or file
            // ...
        }

        using (_logger.StartMeasurement("Data Transformation"))
        {
            // Transform and validate data
            // ...
        }

        using (_logger.StartMeasurement("Data Persistence"))
        {
            // Save processed data
            // ...
        }
    }
}
```

## Advanced Usage

### Custom Log Levels

By default, measurements are logged at `Information` level. You can customize the completion log level:

```csharp
// Log at Debug level
using (_logger.StartMeasurement("Background Task", LogLevel.Debug))
{
    // Background processing
}

// Log at Warning level for critical sections
using (_logger.StartMeasurement("Critical Operation", LogLevel.Warning))
{
    // Critical code
}

// Disable logging completely (measurement still happens)
using (_logger.StartMeasurement("Silent Operation", LogLevel.None))
{
    // No log output
}
```

### Debug Information

Enable detailed caller information for debugging purposes:

```csharp
// Print caller information (method name, file path, line number)
using (_logger.StartMeasurement(
    "Detailed Operation",
    printDebugInformation: true))
{
    // Your code here
}

// By default (null), debug information is only printed on exceptions
using (_logger.StartMeasurement("Standard Operation"))
{
    // Debug info appears only if an exception occurs
}
```

### Nested Measurements

Measurements can be nested to analyze hierarchical operations:

```csharp
using (_logger.StartMeasurement("Complete Workflow"))
{
    using (_logger.StartMeasurement("Step 1: Initialization"))
    {
        // Initialize resources
    }

    using (_logger.StartMeasurement("Step 2: Processing"))
    {
        // Process data
    }

    using (_logger.StartMeasurement("Step 3: Finalization"))
    {
        // Clean up
    }
}
```

## API Reference

### StartMeasurement Method

```csharp
public static IDisposable StartMeasurement(
    this ILogger logger,
    string identifier,
    LogLevel? completionLevel = null,
    bool? printDebugInformation = null,
    [CallerMemberName] string callerMemberName = "",
    [CallerFilePath] string callerFilePath = "",
    [CallerLineNumber] int callerLineNumber = 0)
```

**Parameters:**

- `logger` - The `ILogger` instance to use for logging
- `identifier` - A descriptive name for the measured operation
- `completionLevel` - Optional log level for completion message (default: `Information`, use `None` to disable logging)
- `printDebugInformation` - Optional flag to print caller information (default: `null` = print only on exceptions)
- `callerMemberName` - Automatically captured calling method name
- `callerFilePath` - Automatically captured source file path
- `callerLineNumber` - Automatically captured line number

**Returns:** An `IDisposable` that completes the measurement when disposed.

## Best Practices

1. **Use Descriptive Identifiers**: Choose clear, meaningful names for your measurements
   ```csharp
   // Good
   using (_logger.StartMeasurement("Customer Order Processing"))
   
   // Avoid
   using (_logger.StartMeasurement("Operation1"))
   ```

2. **Appropriate Log Levels**: Match log levels to operation importance
   - `Debug`: Detailed diagnostic measurements
   - `Information`: Standard operation measurements (default)
   - `Warning`: Critical performance sections
   - `None`: Measurement without logging

3. **Minimize Measurement Overhead**: Don't measure trivial operations in tight loops
   ```csharp
   // Good: Measure the entire batch
   using (_logger.StartMeasurement("Process 1000 Items"))
   {
       foreach (var item in items)
       {
           ProcessItem(item);
       }
   }
   
   // Avoid: Measuring each iteration
   foreach (var item in items)
   {
       using (_logger.StartMeasurement($"Process Item {item.Id}"))
       {
           ProcessItem(item);
       }
   }
   ```

4. **Exception Safety**: Measurements are automatically completed even if exceptions occur
   ```csharp
   using (_logger.StartMeasurement("Risky Operation"))
   {
       // Measurement logs elapsed time even if an exception is thrown
       RiskyMethod();
   }
   ```

## Supported Frameworks

- .NET 6.0
- .NET 8.0
- .NET 9.0
- .NET 10.0

## Dependencies

- Microsoft.Extensions.Logging.Abstractions >= 10.0.0
- NetEvolve.Arguments >= 2.0.0

## License

This project is licensed under the MIT License.