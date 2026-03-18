namespace TdsService.Domain.Exceptions;

public class InvalidPanNumberException : DomainException
{
    public InvalidPanNumberException(string message) : base(message) { }
}
