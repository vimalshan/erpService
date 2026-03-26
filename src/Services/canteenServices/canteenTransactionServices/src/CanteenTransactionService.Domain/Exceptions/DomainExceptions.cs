namespace CanteenTransactionService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class TransactionNotFoundException : DomainException
{
    public TransactionNotFoundException(long serialNumber)
        : base($"Canteen transaction with serial number {serialNumber} was not found.") { }
}

public class DuplicateTransactionException : DomainException
{
    public DuplicateTransactionException(long serialNumber)
        : base($"Canteen transaction with serial number {serialNumber} already exists.") { }
}

public class BatchNotFoundException : DomainException
{
    public BatchNotFoundException(long batchNumber)
        : base($"Batch with number {batchNumber} was not found.") { }
}
