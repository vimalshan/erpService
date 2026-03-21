namespace RackingSystem.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.") { }
}

public class DuplicateEntityException : DomainException
{
    public DuplicateEntityException(string entityName, string identifier)
        : base($"Entity '{entityName}' with identifier '{identifier}' already exists.") { }
}

public class ValidationDomainException : DomainException
{
    public ValidationDomainException(string message) : base(message) { }
}
