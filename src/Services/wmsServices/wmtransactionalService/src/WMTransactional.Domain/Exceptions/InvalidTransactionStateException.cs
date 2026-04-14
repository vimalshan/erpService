namespace WMTransactional.Domain.Exceptions;

public class InvalidTransactionStateException : Exception
{
    public string EntityType { get; }
    public string CurrentStatus { get; }
    public string AttemptedAction { get; }

    public InvalidTransactionStateException(string entityType, string currentStatus, string attemptedAction)
        : base($"Cannot {attemptedAction} {entityType} in status '{currentStatus}'.")
    {
        EntityType = entityType;
        CurrentStatus = currentStatus;
        AttemptedAction = attemptedAction;
    }
}
