namespace ExpenseService.Domain.ValueObjects;

public sealed record ExpenseCode
{
    public long Code { get; init; }

    public ExpenseCode(long code)
    {
        if (code <= 0)
            throw new ArgumentException("Expense code must be positive.", nameof(code));
        Code = code;
    }
}
