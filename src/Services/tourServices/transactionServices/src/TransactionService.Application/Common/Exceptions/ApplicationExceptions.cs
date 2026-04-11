namespace TransactionService.Application.Common.Exceptions;

public sealed class ValidationException(IDictionary<string, string[]> errors)
    : Exception("One or more validation failures occurred.")
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}

public sealed class NotFoundException(string message) : Exception(message);
