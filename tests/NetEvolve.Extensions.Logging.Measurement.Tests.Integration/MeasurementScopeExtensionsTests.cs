namespace NetEvolve.Extensions.Logging.Measurement.Tests.Integration;

using System;
using System.Threading.Tasks;
using Meziantou.Extensions.Logging.Xunit;
using Xunit;
using Xunit.Abstractions;

public class MeasurementScopeExtensionsTests
{
    private readonly ITestOutputHelper _output;

    public MeasurementScopeExtensionsTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task StartMeasurement_Execution_Expected()
    {
        var logger = XUnitLogger.CreateLogger<MeasurementScopeExtensionsTests>(_output);

        using (logger.StartMeasurement(nameof(StartMeasurement_Execution_Expected)))
        {
            await Task.Delay(100);
        }

        Assert.True(true);
    }

    [Fact]
    public async Task StartMeasurement_Failed_Expected()
    {
        var logger = XUnitLogger.CreateLogger<MeasurementScopeExtensionsTests>(_output);

        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            using (logger.StartMeasurement(nameof(StartMeasurement_Failed_Expected)))
            {
                await Task.Delay(100);
                throw new InvalidOperationException();
            }
        });
    }
}
