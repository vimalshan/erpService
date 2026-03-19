namespace InvoiceProcessing.Domain.ValueObjects;

public record Money(decimal Amount, int CurrencyId)
{
    public static Money Zero(int currencyId) => new(0, currencyId);
}
