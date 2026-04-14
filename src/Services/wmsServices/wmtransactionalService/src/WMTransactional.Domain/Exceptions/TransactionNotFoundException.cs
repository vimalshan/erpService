namespace WMTransactional.Domain.Exceptions;

public class TransactionNotFoundException : Exception
{
    public string EntityType { get; }
    public object Id { get; }

    public TransactionNotFoundException(string entityType, object id)
        : base($"{entityType} with ID '{id}' was not found.")
    {
        EntityType = entityType;
        Id = id;
    }
}
