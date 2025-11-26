# NetEvolve.Logging.Measurement

[![NuGet](https://img.shields.io/nuget/v/NetEvolve.Logging.Measurement.svg)](https://www.nuget.org/packages/NetEvolve.Logging.Measurement/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NetEvolve.Logging.Measurement.svg)](https://www.nuget.org/packages/NetEvolve.Logging.Measurement/)
[![License](https://img.shields.io/github/license/dailydevops/logging.measurement)](LICENSE)

A lightweight extension library for [Microsoft.Extensions.Logging](https://learn.microsoft.com/aspnet/core/fundamentals/logging/) that enables simple and efficient time measurement of code blocks with automatic logging.

## Features

- ⏱️ **Simple Time Measurement** - Measure execution time of code blocks using intuitive `using` statements
- 📊 **Configurable Log Levels** - Control log output granularity with customizable completion levels
- 🔍 **Debug Information** - Optional caller information (member name, file path, line number) for detailed diagnostics
- 🎯 **Zero Overhead** - Minimal performance impact with efficient measurement implementation
- 🔧 **Flexible Integration** - Seamlessly integrates with existing `ILogger` implementations

## Installation

```bash
dotnet add package NetEvolve.Logging.Measurement
```

## Quick Start

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

    public void ProcessData()
    {
        using (_logger.StartMeasurement("Data Processing"))
        {
            // Your code here
            // Execution time will be logged automatically when the scope is disposed
        }
    }
}
```

## Documentation

For detailed usage examples and advanced scenarios, see the [package documentation](src/NetEvolve.Logging.Measurement/README.md).

## Requirements

- .NET 6.0 or later
- Microsoft.Extensions.Logging.Abstractions 10.0.0 or compatible version

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.