using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace ReviewService.Application.Behaviours;

public sealed class CircuitBreakerBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<CircuitBreakerBehaviour<TRequest, TResponse>> _logger;

    private static readonly ResiliencePipeline Pipeline = new ResiliencePipelineBuilder()
        .AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            SamplingDuration = TimeSpan.FromSeconds(10),
            MinimumThroughput = 5,
            BreakDuration = TimeSpan.FromSeconds(30),
            OnOpened = args =>
            {
                Console.WriteLine($"Circuit breaker opened. Duration: {args.BreakDuration}");
                return ValueTask.CompletedTask;
            },
            OnClosed = _ =>
            {
                Console.WriteLine("Circuit breaker closed.");
                return ValueTask.CompletedTask;
            }
        })
        .Build();

    public CircuitBreakerBehaviour(ILogger<CircuitBreakerBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await Pipeline.ExecuteAsync(
            async ct => await next(ct), cancellationToken);
    }
}
