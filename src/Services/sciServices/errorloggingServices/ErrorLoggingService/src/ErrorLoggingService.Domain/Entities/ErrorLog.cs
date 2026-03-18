using ErrorLoggingService.Domain.Common;
using ErrorLoggingService.Domain.Events;

namespace ErrorLoggingService.Domain.Entities;

public class ErrorLog : AggregateRoot
{
    public int Id { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? StoredProcedureName { get; private set; }
    public int? ErrorReference { get; private set; }
    public DateTime? ErrorDate { get; private set; }

    private ErrorLog() { }

    public static ErrorLog Create(string? errorMessage, string? storedProcedureName, int? errorReference)
    {
        var log = new ErrorLog
        {
            ErrorMessage = errorMessage,
            StoredProcedureName = storedProcedureName,
            ErrorReference = errorReference,
            ErrorDate = DateTime.UtcNow
        };
        log.AddDomainEvent(new ErrorLoggedEvent(log));
        return log;
    }
}
