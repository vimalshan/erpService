namespace LetTransactionService.Domain.Exceptions;

public class LetNotFoundException : LetDomainException
{
    public LetNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.") { }
}
