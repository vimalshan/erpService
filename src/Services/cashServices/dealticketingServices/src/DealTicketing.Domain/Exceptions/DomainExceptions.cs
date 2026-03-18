namespace DealTicketing.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message);

public class DealNotFoundException(long dealId)
    : DomainException($"Deal with ID {dealId} was not found.");

public class DealBatchNotFoundException(long batchId)
    : DomainException($"Deal batch with ID {batchId} was not found.");

public class InvalidDealStatusTransitionException(string from, string to)
    : DomainException($"Cannot transition deal approval status from '{from}' to '{to}'.");

public class DealAlreadySettledException(long dealId)
    : DomainException($"Deal {dealId} has already been settled.");

public class BankNotFoundException(long bankId)
    : DomainException($"Bank with ID {bankId} was not found.");
