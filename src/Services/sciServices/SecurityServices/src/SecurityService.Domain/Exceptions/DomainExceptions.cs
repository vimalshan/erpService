namespace SecurityService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(long userId)
        : base($"User with ID '{userId}' was not found.") { }
}

public class RoleNotFoundException : DomainException
{
    public RoleNotFoundException(long roleId)
        : base($"Role with ID '{roleId}' was not found.") { }
}

public class DuplicateRoleAssignmentException : DomainException
{
    public DuplicateRoleAssignmentException(long userId, long roleId)
        : base($"User '{userId}' already has role '{roleId}' assigned.") { }
}

public class InvalidUserCodeException : DomainException
{
    public InvalidUserCodeException(string code)
        : base($"User code '{code}' is invalid. It must be non-empty and up to 25 characters.") { }
}
