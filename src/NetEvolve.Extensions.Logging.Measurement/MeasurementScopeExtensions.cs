namespace NetEvolve.Extensions.Logging.Measurement;

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using NetEvolve.Arguments;

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
