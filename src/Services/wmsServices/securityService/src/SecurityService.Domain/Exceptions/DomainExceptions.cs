namespace SecurityService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entity, object key)
        : base($"Entity \"{entity}\" ({key}) was not found.") { }
}
