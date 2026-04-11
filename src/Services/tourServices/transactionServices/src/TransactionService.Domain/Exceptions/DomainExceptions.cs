namespace TransactionService.Domain.Exceptions;

public class DomainException(string message) : Exception(message);

public class JournalVoucherNotFoundException(long id)
    : DomainException($"Journal Voucher with ID '{id}' was not found.");

public class JournalVoucherAlreadyPostedException(long id)
    : DomainException($"Journal Voucher '{id}' has already been posted.");

public class TravelBatchNotFoundException(string id)
    : DomainException($"Travel Batch with ID '{id}' was not found.");

public class TravelBatchInvalidStateException(string id, string currentStatus, string attemptedAction)
    : DomainException($"Travel Batch '{id}' cannot be {attemptedAction} in status '{currentStatus}'.");

public class EmployeePaymentNotFoundException(long id)
    : DomainException($"Employee Payment with ID '{id}' was not found.");

public class AirlineInvoiceNotFoundException(string id)
    : DomainException($"Airline Invoice with ID '{id}' was not found.");
