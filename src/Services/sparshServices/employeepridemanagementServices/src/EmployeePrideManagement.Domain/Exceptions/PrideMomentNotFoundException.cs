namespace EmployeePrideManagement.Domain.Exceptions;

public class PrideMomentNotFoundException : Exception
{
    public PrideMomentNotFoundException(decimal id)
        : base($"Pride moment with ID {id} was not found.")
    {
    }
}
