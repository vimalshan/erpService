namespace Hr.ApiGateway.Telemetry;

public sealed class GatewayMetrics
{
    private long _totalRequests;
    private long _successfulResponses;
    private long _failedResponses;
    private long _totalDurationMs;

    public void Track(int statusCode, long elapsedMs)
    {
        Interlocked.Increment(ref _totalRequests);
        Interlocked.Add(ref _totalDurationMs, elapsedMs);

        if (statusCode is >= 200 and < 500)
        {
            Interlocked.Increment(ref _successfulResponses);
            return;
        }

        Interlocked.Increment(ref _failedResponses);
    }

    public object Snapshot()
    {
        var total = Interlocked.Read(ref _totalRequests);
        var avgMs = total == 0 ? 0 : Interlocked.Read(ref _totalDurationMs) / total;

        return new
        {
            totalRequests = total,
            successfulResponses = Interlocked.Read(ref _successfulResponses),
            failedResponses = Interlocked.Read(ref _failedResponses),
            averageLatencyMs = avgMs
        };
    }
}
