namespace LocationService.Domain.Exceptions
{
    /// <summary>
    /// Base domain exception
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when entity is not found
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, long id)
            : base($"{entityName} with ID {id} was not found.") { }

        public EntityNotFoundException(string entityName, string code)
            : base($"{entityName} with code '{code}' was not found.") { }
    }

    /// <summary>
    /// Exception thrown when entity already exists
    /// </summary>
    public class EntityAlreadyExistsException : DomainException
    {
        public EntityAlreadyExistsException(string entityName, string code)
            : base($"{entityName} with code '{code}' already exists.") { }
    }

    /// <summary>
    /// Exception thrown when invalid operation is performed
    /// </summary>
    public class InvalidOperationException : DomainException
    {
        public InvalidOperationException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when business rule is violated
    /// </summary>
    public class BusinessRuleException : DomainException
    {
        public string? Code { get; set; }

        public BusinessRuleException(string message, string? code = null)
            : base(message)
        {
            Code = code;
        }
    }
}
