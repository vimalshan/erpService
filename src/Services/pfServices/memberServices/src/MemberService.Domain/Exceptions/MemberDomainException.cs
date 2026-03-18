namespace MemberService.Domain.Exceptions;

public class MemberDomainException : Exception
{
    public MemberDomainException(string message) : base(message) { }
    public MemberDomainException(string message, Exception inner) : base(message, inner) { }
}
