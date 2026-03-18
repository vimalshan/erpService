namespace DocumentService.Domain.Exceptions;

public sealed class LoanDocumentNotFoundException : Exception
{
    public LoanDocumentNotFoundException(long id)
        : base($"Loan document with ID {id} was not found.") { }
}
