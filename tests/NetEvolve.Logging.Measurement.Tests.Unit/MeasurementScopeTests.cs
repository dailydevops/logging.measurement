namespace NetEvolve.Logging.Measurement.Tests.Unit;

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

public class MeasurementScopeTests
{
    [Fact]
    public void Constructor_WithLoggerNull_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger logger = null!;

        // Act
        void Act() =>
            _ = new MeasurementScope(
                logger,
                nameof(Constructor_WithLoggerNull_ThrowsArgumentNullException),
                default,
                default,
                null,
                "",
                "",
                0
            );

        // Assert
        _ = Assert.Throws<ArgumentNullException>("logger", Act);
    }

    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceData))]
    public void Constructor_WithNameInvalid_ThrowsArgumentException(
        Type exceptionType,
        string? name
    )
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeTests>.Instance;

        // Act
        void Act() => _ = new MeasurementScope(logger, name, default, default, null, "", "", 0);

        // Assert
        var ex = (ArgumentException)Assert.Throws(exceptionType, Act);
        Assert.IsType(exceptionType, ex);
        Assert.Equal("name", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceData))]
    public void Constructor_WithMemberNameInvalid_ThrowsArgumentException(
        Type exceptionType,
        string? memberName
    )
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeTests>.Instance;

        // Act
        void Act() =>
            _ = new MeasurementScope(
                logger,
                nameof(Constructor_WithMemberNameInvalid_ThrowsArgumentException),
                default,
                default,
                null,
                memberName,
                "",
                0
            );

        // Assert
        var ex = (ArgumentException)Assert.Throws(exceptionType, Act);
        Assert.IsType(exceptionType, ex);
        Assert.Equal("memberName", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceData))]
    public void Constructor_WithFilePathInvalid_ThrowsArgumentException(
        Type exceptionType,
        string? filePath
    )
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeTests>.Instance;

        // Act
        void Act() =>
            _ = new MeasurementScope(
                logger,
                nameof(Constructor_WithFilePathInvalid_ThrowsArgumentException),
                default,
                default,
                null,
                nameof(Constructor_WithFilePathInvalid_ThrowsArgumentException),
                filePath,
                0
            );

        // Assert
        var ex = (ArgumentException)Assert.Throws(exceptionType, Act);
        Assert.IsType(exceptionType, ex);
        Assert.Equal("filePath", ex.ParamName);
    }

    [Fact]
    public void Constructor_WithLineNumberInvalid_ThrowsArgumentException()
    {
        // Arrange
        ILogger logger = NullLogger<MeasurementScopeTests>.Instance;

        // Act
        void Act() =>
            _ = new MeasurementScope(
                logger,
                nameof(Constructor_WithLineNumberInvalid_ThrowsArgumentException),
                default,
                default,
                null,
                nameof(Constructor_WithLineNumberInvalid_ThrowsArgumentException),
                nameof(Constructor_WithLineNumberInvalid_ThrowsArgumentException),
                -1
            );

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>("lineNumber", Act);
    }

    public static TheoryData<Type, string?> NullOrWhiteSpaceData =>
        new TheoryData<Type, string?>
        {
            { typeof(ArgumentNullException), null },
            { typeof(ArgumentException), "" },
            { typeof(ArgumentException), " " },
            { typeof(ArgumentException), "\t" }
        };
}
