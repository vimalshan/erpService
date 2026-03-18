namespace TdsService.Domain.Exceptions;

public class TdsFileNotFoundException : DomainException
{
    public TdsFileNotFoundException(long fileId)
        : base($"TDS File with ID '{fileId}' was not found.") { }
}
