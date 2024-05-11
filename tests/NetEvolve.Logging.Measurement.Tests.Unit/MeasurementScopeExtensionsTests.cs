namespace NetEvolve.Logging.Measurement.Tests.Unit;

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

public class MeasurementScopeExtensionsTests
{
    [Fact]
    public void StartMeasurement_WithLoggerNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = null!;

        // Act
        void Act() =>
            logger.StartMeasurement(
                nameof(StartMeasurement_WithLoggerNull_ThrowsArgumentNullException)
            );

        // Assert
        _ = Assert.Throws<ArgumentNullException>("logger", Act);
    }

    [Fact]
    public void StartMeasurement_WithNameNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        void Act() => logger.StartMeasurement(null!, LogLevel.Information);

        // Assert
        _ = Assert.Throws<ArgumentNullException>("name", Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void StartMeasurement_WithNameEmptyOrWhiteSpace_ThrowsArgumentException(string name)
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        void Act() => logger.StartMeasurement(name, LogLevel.Information);

        // Assert
        _ = Assert.Throws<ArgumentException>(nameof(name), Act);
    }
}
