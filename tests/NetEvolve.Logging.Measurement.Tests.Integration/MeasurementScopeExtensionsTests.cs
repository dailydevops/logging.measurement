namespace NetEvolve.Logging.Measurement.Tests.Integration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvolve.Logging.Abstractions;
using NetEvolve.Logging.XUnit;
using Xunit;
using Xunit.Abstractions;

public class MeasurementScopeExtensionsTests
{
    private readonly ITestOutputHelper _output;
    private readonly XUnitLoggerOptions _options = XUnitLoggerOptions.Default;

    public MeasurementScopeExtensionsTests(ITestOutputHelper output) => _output = output;

    [Theory]
    [MemberData(nameof(StartMeasurementData))]
    public async Task StartMeasurement_TheoryWithoutExceptions_Expected(
        LogLevel completionLevel,
        LogLevel? failedLevel,
        bool? printDebugInformation
    )
    {
        var logger = XUnitLogger.CreateLogger<MeasurementScopeExtensionsTests>(
            _output,
            options: _options
        );

        using (
            logger.StartMeasurement(
                nameof(StartMeasurement_TheoryWithoutExceptions_Expected),
                completionLevel,
                failedLevel,
                printDebugInformation
            )
        )
        {
            await Task.Delay(25);
        }

        Assert.Collection(
            logger.LoggedMessages,
            GetExpectedEntries(
                false,
                nameof(StartMeasurement_TheoryWithoutExceptions_Expected),
                completionLevel,
                failedLevel,
                printDebugInformation
            )
        );
    }

    [Theory]
    [MemberData(nameof(StartMeasurementData))]
    public async Task StartMeasurement_TheoryWithExceptions_Expected(
        LogLevel completionLevel,
        LogLevel? failedLevel,
        bool? printDebugInformation
    )
    {
        var logger = XUnitLogger.CreateLogger<MeasurementScopeExtensionsTests>(
            _output,
            options: _options
        );

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            using (
                logger.StartMeasurement(
                    nameof(StartMeasurement_TheoryWithExceptions_Expected),
                    completionLevel,
                    failedLevel,
                    printDebugInformation
                )
            )
            {
                await Task.Delay(25);
                throw new InvalidOperationException();
            }
        });

        Assert.NotNull(ex);
        Assert.Collection(
            logger.LoggedMessages,
            GetExpectedEntries(
                true,
                nameof(StartMeasurement_TheoryWithExceptions_Expected),
                completionLevel,
                failedLevel,
                printDebugInformation
            )
        );
    }

    private Action<LoggedMessage>[] GetExpectedEntries(
        bool throwError,
        string name,
        LogLevel completionLevel,
        LogLevel? failedLevel,
        bool? printDebugInformation
    )
    {
        var result = new List<Action<LoggedMessage>>
        {
            entry =>
            {
                Assert.Equal(completionLevel, entry.LogLevel);
                Assert.Contains(
                    $"Measurement `{name}` started.",
                    entry.Message,
                    StringComparison.Ordinal
                );
            },
            entry =>
            {
                var level = completionLevel;
                var message = $"Measurement `{name}` completed in ";

                if (throwError)
                {
                    level = failedLevel ?? completionLevel;
                    message = $"Measurement `{name}` failed with exception after ";
                }

                Assert.Equal(level, entry.LogLevel);
                Assert.StartsWith(message, entry.Message, StringComparison.Ordinal);
            }
        };

        if (
            (printDebugInformation.HasValue && printDebugInformation.Value)
            || (!printDebugInformation.HasValue && throwError)
        )
        {
            result.Add(entry =>
            {
                Assert.Equal(LogLevel.Debug, entry.LogLevel);
                Assert.Contains(
                    $"Measurement `{name}` Debug Information - MemberName: ",
                    entry.Message,
                    StringComparison.Ordinal
                );
            });
        }

        return [.. result];
    }

    public static TheoryData<LogLevel, LogLevel?, bool?> StartMeasurementData
    {
        get
        {
            var data = new TheoryData<LogLevel, LogLevel?, bool?>();

            foreach (
                var completionLevel in new LogLevel[] { LogLevel.Information, LogLevel.Warning }
            )
            {
                foreach (var printDebugInformation in new bool?[] { null, false, true })
                {
                    foreach (var failedLevel in new LogLevel?[] { null, LogLevel.Information })
                    {
                        data.Add(completionLevel, failedLevel, printDebugInformation);
                    }
                }
            }

            return data;
        }
    }
}
