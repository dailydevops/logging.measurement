namespace NetEvolve.Logging.Measurement.Tests.Unit;

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetEvolve.Logging.Abstractions;
using Xunit;

public class MeasurementScopeExtensionsTests
{
    [Fact]
    public void StartMeasurement_WithLoggerNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = null!;

        // Act
        void Act() => logger.StartMeasurement(nameof(StartMeasurement_WithLoggerNull_ThrowsArgumentNullException));

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
        _ = Assert.Throws<ArgumentNullException>("identifier", Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void StartMeasurement_WithNameEmptyOrWhiteSpace_ThrowsArgumentException(string identifier)
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        void Act() => logger.StartMeasurement(identifier, LogLevel.Information);

        // Assert
        _ = Assert.Throws<ArgumentException>(nameof(identifier), Act);
    }

    [Fact]
    public void StartMeasurement_WithLevelNone_ReturnsNullScope()
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        var result = logger.StartMeasurement("identifier", LogLevel.None);

        // Assert
        Assert.Same(NullScope.Instance, result);
    }

    [Fact]
    public void StartMeasurement_WithLevelNoneAndLoggerNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = null!;

        // Act
        void Act() => logger.StartMeasurement("identifier", LogLevel.None);

        // Assert
        _ = Assert.Throws<ArgumentNullException>("logger", Act);
    }

    [Fact]
    public void StartMeasurement_WithLevelNoneAndNameNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        void Act() => logger.StartMeasurement(null!, LogLevel.None);

        // Assert
        _ = Assert.Throws<ArgumentNullException>("identifier", Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("	")]
    public void StartMeasurement_WithLevelNoneAndNameEmptyOrWhiteSpace_ThrowsArgumentException(string identifier)
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeExtensionsTests>.Instance;

        // Act
        void Act() => logger.StartMeasurement(identifier, LogLevel.None);

        // Assert
        _ = Assert.Throws<ArgumentException>(nameof(identifier), Act);
    }
}
