# NetEvolve.Logging.Measurement

This library provides a logging implementation for measuring the time taken to execute a block of code. This doesn't replace a proper profiler, but it can be useful for quickly identifying bottlenecks in your code.

## Installation
```bash
dotnet add package NetEvolve.Logging.Measurement
```

## Usage

You can use the extension methods on the `ILogger` instance to measure the time taken to execute a block of code.

```csharp
using Microsoft.Extensions.Logging;
using NetEvolve.Logging.Measurement;

public sealed class Example
{
    private readonly ILogger<Example> _logger;

    public Example(ILogger<Example> logger)
    {
        _logger = logger;
    }

    public void Method()
    {
        using (_logger.Measure("Block 1"))
        {
            // Block 1
            ...
        }

        using (_logger.Measure("Block 2"))
        {
            // Block 2
            ...
        }
    }
}
```