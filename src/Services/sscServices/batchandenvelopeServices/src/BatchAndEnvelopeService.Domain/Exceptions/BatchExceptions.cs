namespace BatchAndEnvelopeService.Domain.Exceptions;

public class BatchDomainException : Exception
{
    public BatchDomainException(string message) : base(message) { }
    public BatchDomainException(string message, Exception inner) : base(message, inner) { }
}

public class BatchNotFoundException : BatchDomainException
{
    public BatchNotFoundException(long batchId) : base($"Batch with ID {batchId} was not found.") { }
}
