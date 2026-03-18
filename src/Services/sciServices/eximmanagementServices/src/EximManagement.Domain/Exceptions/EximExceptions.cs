namespace EximManagement.Domain.Exceptions;

public class EximDomainException : Exception
{
    public EximDomainException(string message) : base(message) { }
    public EximDomainException(string message, Exception inner) : base(message, inner) { }
}

public class EximNotFoundException : EximDomainException
{
    public EximNotFoundException(string entity, object key)
        : base($"Entity '{entity}' with key '{key}' was not found.") { }
}

public class EximValidationException : EximDomainException
{
    public EximValidationException(string message) : base(message) { }
}
