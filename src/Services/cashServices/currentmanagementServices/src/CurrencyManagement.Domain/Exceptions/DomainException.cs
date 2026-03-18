namespace CurrencyManagement.Domain.Exceptions;

/// <summary>
/// Base exception for all domain-level exceptions
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when a currency is not found
/// </summary>
public class CurrencyNotFoundException : DomainException
{
    public CurrencyNotFoundException(long currencyId) : base($"Currency with ID {currencyId} not found") { }
}

/// <summary>
/// Thrown when trying to set an exchange rate with invalid values
/// </summary>
public class InvalidExchangeRateException : DomainException
{
    public InvalidExchangeRateException(string message) : base(message) { }
}

/// <summary>
/// Thrown when trying to map currencies that don't exist
/// </summary>
public class InvalidCurrencyMappingException : DomainException
{
    public InvalidCurrencyMappingException(string message) : base(message) { }
}
