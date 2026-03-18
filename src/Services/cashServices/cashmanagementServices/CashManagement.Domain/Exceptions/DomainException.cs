namespace CashManagement.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class InsufficientCashException : DomainException
{
    public decimal AvailableBalance { get; }
    public decimal RequestedAmount { get; }

    public InsufficientCashException(decimal available, decimal requested)
        : base($"Insufficient cash. Available: {available}, Requested: {requested}")
    {
        AvailableBalance = available;
        RequestedAmount = requested;
    }
}

public class InvalidTransactionTypeException : DomainException
{
    public InvalidTransactionTypeException(string type)
        : base($"Invalid transaction type: '{type}'.") { }
}

public class DuplicateChequeException : DomainException
{
    public DuplicateChequeException(string chequeNumber)
        : base($"Cheque number '{chequeNumber}' already exists for this account.") { }
}

public class ChequeStatusTransitionException : DomainException
{
    public ChequeStatusTransitionException(string from, string to)
        : base($"Cannot transition cheque from status '{from}' to '{to}'.") { }
}
