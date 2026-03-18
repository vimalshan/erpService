namespace ErrorLoggingService.Infrastructure.Messaging.Events;

public class ErrorLoggedMessage
{
    public int ErrorLogId { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StoredProcedureName { get; set; }
    public int? ErrorReference { get; set; }
    public DateTime? ErrorDate { get; set; }

    public ErrorLoggedMessage() { }

    public ErrorLoggedMessage(int errorLogId, string? errorMessage, string? storedProcedureName, int? errorReference, DateTime? errorDate)
    {
        ErrorLogId = errorLogId;
        ErrorMessage = errorMessage;
        StoredProcedureName = storedProcedureName;
        ErrorReference = errorReference;
        ErrorDate = errorDate;
    }
}
