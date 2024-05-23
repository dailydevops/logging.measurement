namespace NetEvolve.Logging.Measurement;

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using NetEvolve.Arguments;
using NetEvolve.Logging.Abstractions;

/// <summary>
/// Extensions for <see cref="ILogger"/> to start a time measurement.
/// </summary>
public static class MeasurementScopeExtensions
{
    internal const LogLevel DefaultLogLevel = LogLevel.Information;

    /// <summary>
    /// Starts the time measurement for the <paramref name="identifier"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> instance.</param>
    /// <param name="identifier">The identifier for the measurement.</param>
    /// <param name="completionLevel">
    /// <para>Optional completion level for the measurement.</para>
    /// <para>If <see langword="null"/> is provided, the default <see cref="LogLevel.Information"/> will be used.</para>
    /// <para>If <see cref="LogLevel.None"/> is provided, the measurement will not be logged.</para>
    /// </param>
    /// <param name="printDebugInformation">
    /// Optional flag to print debug information. If <see langword="null"/> is provided, the values will be printed in cases of exceptions.
    /// </param>
    /// <param name="callerMemberName">Name of the calling member.</param>
    /// <param name="callerFilePath">FilePath of the calling member.</param>
    /// <param name="callerLineNumber">Line number of the calling member.</param>
    /// <returns>A disposable object that will complete the measurement when disposed.</returns>
    public static IDisposable StartMeasurement(
        this ILogger logger,
        string identifier,
        LogLevel? completionLevel = null,
        bool? printDebugInformation = null,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    )
    {
        ArgumentNullException.ThrowIfNull(logger);
        Argument.ThrowIfNullOrWhiteSpace(identifier);

        if (completionLevel is LogLevel.None)
        {
            return NullScope.Instance;
        }

        return new MeasurementScope(
            logger,
            identifier,
            completionLevel ?? DefaultLogLevel,
            printDebugInformation,
            callerMemberName,
            callerFilePath,
            callerLineNumber
        );
    }
}
