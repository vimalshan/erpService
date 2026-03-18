using ErrorLoggingService.Domain.Common;
using ErrorLoggingService.Domain.Entities;
using ErrorLoggingService.Domain.Events;

namespace ErrorLoggingService.Domain.Aggregates;

/// <summary>
/// Aggregate root for the ErrorLog bounded context.
/// Wraps creation and state-change logic for ErrorLog.
/// </summary>
public sealed class ErrorLogAggregate : AggregateRoot
{
    public ErrorLog ErrorLog { get; private set; }

    private ErrorLogAggregate(ErrorLog errorLog)
    {
        ErrorLog = errorLog;
    }

    public static ErrorLogAggregate Create(string? errorMessage, string? storedProcedureName, int? errorReference)
    {
        var errorLog = ErrorLog.Create(errorMessage, storedProcedureName, errorReference);
        var aggregate = new ErrorLogAggregate(errorLog);
        aggregate.AddDomainEvent(new ErrorLoggedEvent(errorLog));
        return aggregate;
    }
}
