using TransactionService.Domain.Events;

namespace TransactionService.API.Middleware;

public class DomainEventPublishingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DomainEventPublishingMiddleware> _logger;

    public DomainEventPublishingMiddleware(RequestDelegate next, ILogger<DomainEventPublishingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IDomainEventPublisher eventPublisher)
    {
        await _next(context);

        _logger.LogInformation($"Domain events middleware executed for {context.Request.Path}");
    }
}
