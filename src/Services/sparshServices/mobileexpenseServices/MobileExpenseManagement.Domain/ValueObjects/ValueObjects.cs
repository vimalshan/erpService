namespace MobileExpenseManagement.Domain.ValueObjects;

/// <summary>
/// Value object for money amount with currency
/// </summary>
public record Money(decimal Amount, string Currency)
{
    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        return new Money(amount, currency);
    }
}

/// <summary>
/// Value object for date range
/// </summary>
public record DateRange(DateTime StartDate, DateTime EndDate)
{
    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date");

        return new DateRange(startDate, endDate);
    }

    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;
}

/// <summary>
/// Value object for expense category
/// </summary>
public record ExpenseCategory(decimal CategoryId, string Name, decimal MaxLimit)
{
    public bool IsWithinLimit(decimal amount) => amount <= MaxLimit;
}
