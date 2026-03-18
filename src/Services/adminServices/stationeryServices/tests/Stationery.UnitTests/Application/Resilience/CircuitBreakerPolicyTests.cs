using FluentAssertions;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Xunit;

namespace Stationery.UnitTests.Application.Resilience;

public class CircuitBreakerPolicyTests
{
    private static AsyncRetryPolicy<HttpResponseMessage> BuildRetryPolicy(int retries = 3)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(retries, attempt => TimeSpan.FromMilliseconds(10 * attempt));

    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> BuildCircuitBreakerPolicy(int threshold = 3)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(threshold, TimeSpan.FromMilliseconds(500));

    [Fact]
    public async Task RetryPolicy_ShouldRetry_OnTransientError()
    {
        int callCount = 0;
        var policy = BuildRetryPolicy(retries: 2);

        var result = await policy.ExecuteAsync(() =>
        {
            callCount++;
            if (callCount < 3)
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable));
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        });

        callCount.Should().Be(3);
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task CircuitBreaker_ShouldOpen_AfterConsecutiveFailures()
    {
        var cbPolicy = BuildCircuitBreakerPolicy(threshold: 2);
        int callCount = 0;

        // Force 2 failures to open the circuit
        for (int i = 0; i < 2; i++)
        {
            try
            {
                await cbPolicy.ExecuteAsync(() =>
                {
                    callCount++;
                    return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
                });
            }
            catch { /* expected */ }
        }

        // Circuit should be open now — next call should throw BrokenCircuitException
        Func<Task> act = () => cbPolicy.ExecuteAsync(() =>
        {
            callCount++;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        });

        await act.Should().ThrowAsync<BrokenCircuitException>();
        cbPolicy.CircuitState.Should().Be(CircuitState.Open);
    }

    [Fact]
    public async Task CircuitBreaker_ShouldReclose_AfterResetTimeout()
    {
        var cbPolicy = BuildCircuitBreakerPolicy(threshold: 2);

        // Open the circuit
        for (int i = 0; i < 2; i++)
        {
            try
            {
                await cbPolicy.ExecuteAsync(() =>
                    Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)));
            }
            catch { }
        }

        cbPolicy.CircuitState.Should().Be(CircuitState.Open);

        // Wait for reset timeout (500ms configured above)
        await Task.Delay(600);

        cbPolicy.CircuitState.Should().Be(CircuitState.HalfOpen);

        // Successful call should close the circuit
        var result = await cbPolicy.ExecuteAsync(() =>
            Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        cbPolicy.CircuitState.Should().Be(CircuitState.Closed);
    }
}
