using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

#nullable enable

namespace MasterData.Infrastructure.Services
{
    /// <summary>
    /// Polly resilience policies for external service calls
    /// </summary>
    public static class ResiliencePolicies
    {
        /// <summary>
        /// Creates a circuit breaker policy
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync<HttpResponseMessage>(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: OnBreak,
                    onReset: OnReset
                );
        }

        /// <summary>
        /// Creates a retry policy with exponential backoff
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync<HttpResponseMessage>(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: OnRetry
                );
        }

        /// <summary>
        /// Creates a timeout policy
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(
                TimeSpan.FromSeconds(10)
            );
        }

        /// <summary>
        /// Creates a combined policy wrapping all policies
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            return Policy.WrapAsync(
                GetTimeoutPolicy(),
                GetRetryPolicy(),
                GetCircuitBreakerPolicy()
            );
        }

        private static void OnBreak(
            DelegateResult<HttpResponseMessage> outcome,
            TimeSpan timespan,
            Context context)
        {
            Console.WriteLine($"Circuit breaker opened. Will retry after {timespan.TotalSeconds} seconds");
        }

        private static void OnReset(Context context)
        {
            Console.WriteLine("Circuit breaker reset");
        }

        private static void OnRetry(
            DelegateResult<HttpResponseMessage> outcome,
            TimeSpan timespan,
            int retryAttempt,
            Context context)
        {
            Console.WriteLine($"Retrying... Attempt {retryAttempt} after {timespan.TotalSeconds} seconds");
        }
    }
}
