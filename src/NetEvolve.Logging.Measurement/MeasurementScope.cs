namespace NetEvolve.Logging.Measurement;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using NetEvolve.Arguments;

internal sealed partial class MeasurementScope : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogLevel _completionLevel;
    private readonly bool? _printDebugInformation;
    private readonly string _identifier;
    private readonly string _memberName;
    private readonly string _filePath;
    private readonly int _lineNumber;
    private readonly Stopwatch _stopWatch;

    internal MeasurementScope(
        ILogger logger,
        string identifier,
        LogLevel completionLevel,
        bool? printDebugInformation,
        string memberName,
        string filePath,
        int lineNumber
    )
    {
        ArgumentNullException.ThrowIfNull(logger);
        Argument.ThrowIfNullOrWhiteSpace(identifier);
        Argument.ThrowIfNullOrWhiteSpace(memberName);
        Argument.ThrowIfNullOrWhiteSpace(filePath);
        Argument.ThrowIfLessThan(lineNumber, 0);

        _logger = logger;
        _completionLevel = completionLevel;
        _printDebugInformation = printDebugInformation;
        _identifier = identifier;

        _memberName = memberName;
        _filePath = filePath;
        _lineNumber = lineNumber;

        _stopWatch = Stopwatch.StartNew();

        LogStart(_completionLevel, _identifier);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stopWatch.Stop();

        var printDebugInformation = _printDebugInformation;

        if (Marshal.GetExceptionPointers() == IntPtr.Zero)
        {
            LogComplete(_completionLevel, _identifier, _stopWatch.ElapsedMilliseconds);
        }
        else
        {
            LogFailed(_completionLevel, _identifier, _stopWatch.ElapsedMilliseconds);
            if (!printDebugInformation.HasValue)
            {
                printDebugInformation = true;
            }
        }

        if (printDebugInformation.HasValue && printDebugInformation.Value)
        {
            LogDebugInformation(_identifier, _memberName, _filePath, _lineNumber);
        }
    }

    [LoggerMessage(EventId = 1, Message = "Measurement `{identifier}` started.")]
    private partial void LogStart(LogLevel level, string identifier);

    [LoggerMessage(
        EventId = 2,
        Message = "Measurement `{identifier}` completed in {elapsedMilliseconds} ms."
    )]
    private partial void LogComplete(LogLevel level, string identifier, long elapsedMilliseconds);

    [LoggerMessage(
        EventId = 3,
        Message = "Measurement `{identifier}` failed with exception in {elapsedMilliseconds} ms."
    )]
    private partial void LogFailed(LogLevel level, string identifier, long elapsedMilliseconds);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Measurement `{identifier}` - Debug Information - MemberName: `{memberName}` FilePath: `{filePath}` LineNumber: `{lineNumber}`"
    )]
    private partial void LogDebugInformation(
        string identifier,
        string memberName,
        string filePath,
        int lineNumber
    );
}
