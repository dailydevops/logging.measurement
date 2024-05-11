namespace NetEvolve.Extensions.Logging.Measurement;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using NetEvolve.Arguments;

internal sealed partial class MeasurementScope : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogLevel _completionLevel;
    private readonly LogLevel _failedLevel;
    private readonly bool? _printDebugInformation;
    private readonly string _name;
    private readonly string _memberName;
    private readonly string _filePath;
    private readonly int _lineNumber;
    private readonly Stopwatch _stopWatch;

    internal MeasurementScope(
        ILogger logger,
        string name,
        LogLevel completionLevel,
        LogLevel failedLevel,
        bool? printDebugInformation,
        string memberName,
        string filePath,
        int lineNumber
    )
    {
        ArgumentNullException.ThrowIfNull(logger);
        Argument.ThrowIfNullOrWhiteSpace(name);
        Argument.ThrowIfNullOrWhiteSpace(memberName);
        Argument.ThrowIfNullOrWhiteSpace(filePath);
        Argument.ThrowIfLessThan(lineNumber, 0);

        _logger = logger;
        _completionLevel = completionLevel;
        _failedLevel = failedLevel;
        _printDebugInformation = printDebugInformation;
        _name = name;

        _memberName = memberName;
        _filePath = filePath;
        _lineNumber = lineNumber;

        _stopWatch = Stopwatch.StartNew();

        LogStart(_completionLevel, _name);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stopWatch.Stop();

        var printDebugInformation = _printDebugInformation;

        if (Marshal.GetExceptionPointers() == IntPtr.Zero)
        {
            LogComplete(_completionLevel, _name, _stopWatch.ElapsedMilliseconds);
        }
        else
        {
            LogFailed(_failedLevel, _name, _stopWatch.ElapsedMilliseconds);
            if (!printDebugInformation.HasValue)
            {
                printDebugInformation = true;
            }
        }

        if (printDebugInformation.HasValue && printDebugInformation.Value)
        {
            LogDebugInformation(_name, _memberName, _filePath, _lineNumber);
        }
    }

    [LoggerMessage(EventId = 1, Message = "Measurement `{name}` started.")]
    private partial void LogStart(LogLevel level, string name);

    [LoggerMessage(
        EventId = 2,
        Message = "Measurement `{name}` completed in {elapsedMilliseconds} ms."
    )]
    private partial void LogComplete(LogLevel level, string name, long elapsedMilliseconds);

    [LoggerMessage(
        EventId = 3,
        Message = "Measurement `{name}` failed with exception after {elapsedMilliseconds} ms."
    )]
    private partial void LogFailed(LogLevel level, string name, long elapsedMilliseconds);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Measurement `{name}` Debug Information - MemberName: `{memberName}` FilePath: `{filePath}` LineNumber: `{lineNumber}`"
    )]
    private partial void LogDebugInformation(
        string name,
        string memberName,
        string filePath,
        int lineNumber
    );
}
