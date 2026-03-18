namespace Shared.Infrastructure.Logging;

using Microsoft.Extensions.Logging;
using Serilog.Context;

/// <summary>
/// Logging extension methods for structured logging
/// </summary>
public static class LoggingExtensions
{
    public static IDisposable EnrichLogContext(string requestId, string? userId = null, string? correlationId = null)
    {
        var disposables = new List<IDisposable>();

        disposables.Add(LogContext.PushProperty("RequestId", requestId));

        if (!string.IsNullOrEmpty(userId))
            disposables.Add(LogContext.PushProperty("UserId", userId));

        if (!string.IsNullOrEmpty(correlationId))
            disposables.Add(LogContext.PushProperty("CorrelationId", correlationId));

        return new CompositeDisposable(disposables);
    }

    public static void LogRequest(this ILogger logger, string method, string path, string requestId)
    {
        logger.LogInformation(
            "HTTP request | Method: {Method} | Path: {Path} | RequestId: {RequestId}",
            method,
            path,
            requestId);
    }

    public static void LogResponse(this ILogger logger, string method, string path, int statusCode, long elapsedMs, string requestId)
    {
        var level = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
        logger.Log(
            level,
            "HTTP response | Method: {Method} | Path: {Path} | StatusCode: {StatusCode} | ElapsedMs: {ElapsedMilliseconds} | RequestId: {RequestId}",
            method,
            path,
            statusCode,
            elapsedMs,
            requestId);
    }

    public static void LogDatabaseQuery(this ILogger logger, string query, long elapsedMs)
    {
        logger.LogDebug(
            "Database query executed | ElapsedMs: {ElapsedMilliseconds} | Query: {Query}",
            elapsedMs,
            query);
    }

    public static void LogEvent(this ILogger logger, string eventName, string eventSource, object? eventData = null)
    {
        logger.LogInformation(
            "Domain event published | Event: {EventName} | Source: {Source}",
            eventName,
            eventSource);
    }

    public static void LogCache(this ILogger logger, string operation, string key, string source = "Redis")
    {
        logger.LogDebug(
            "Cache {Operation} | Key: {CacheKey} | Source: {Source}",
            operation,
            key,
            source);
    }
}

/// <summary>
/// Composite disposable for managing multiple IDisposable objects
/// </summary>
public class CompositeDisposable : IDisposable
{
    private readonly List<IDisposable> _disposables;

    public CompositeDisposable(List<IDisposable> disposables)
    {
        _disposables = disposables;
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable?.Dispose();
        }

        _disposables.Clear();
    }
}
