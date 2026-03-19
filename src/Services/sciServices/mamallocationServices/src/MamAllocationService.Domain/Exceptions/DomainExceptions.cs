namespace MamAllocationService.Domain.Exceptions;

public class AllocationNotFoundException : Exception
{
    public AllocationNotFoundException(DateTime date, int rmCode)
        : base($"Allocation not found for Date={date:yyyy-MM-dd}, RM={rmCode}") { }
}

public class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message) { }
}
