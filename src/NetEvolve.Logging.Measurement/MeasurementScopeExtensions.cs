namespace NetEvolve.Logging.Measurement;

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using NetEvolve.Arguments;
using NetEvolve.Logging.Abstractions;

public static class MeasurementScopeExtensions
{
    public static IDisposable StartMeasurement(
        this ILogger logger,
        string name,
        LogLevel completionLevel = LogLevel.Information,
        LogLevel? failedLevel = null,
        bool? printDebugInformation = null,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    )
    {
        ArgumentNullException.ThrowIfNull(logger);
        Argument.ThrowIfNullOrWhiteSpace(name);

        if (completionLevel is LogLevel.None)
        {
            return NullScope.Instance;
        }

        return new MeasurementScope(
            logger,
            name,
            completionLevel,
            failedLevel ?? completionLevel,
            printDebugInformation,
            callerMemberName,
            callerFilePath,
            callerLineNumber
        );
    }
}
