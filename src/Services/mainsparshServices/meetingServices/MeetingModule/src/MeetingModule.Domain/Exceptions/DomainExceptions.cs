namespace MeetingModule.Domain.Exceptions;

public class MeetingDomainException : Exception
{
    public MeetingDomainException(string message) : base(message) { }
    public MeetingDomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class EntityNotFoundException : MeetingDomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.") { }
}
