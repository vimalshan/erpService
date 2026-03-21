using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class ExpenseCurrency : AggregateRoot<string>
{
    public string CurrencyName { get; private set; } = string.Empty;
    public string CurrencyShortName { get; private set; } = string.Empty;
    public string CurrencySymbol { get; private set; } = string.Empty;

    private ExpenseCurrency() { }

    public static ExpenseCurrency Create(string code, string name, string shortName, string symbol)
    {
        return new ExpenseCurrency
        {
            Id = code,
            CurrencyName = name,
            CurrencyShortName = shortName,
            CurrencySymbol = symbol
        };
    }

    public void Update(string name, string shortName, string symbol)
    {
        CurrencyName = name;
        CurrencyShortName = shortName;
        CurrencySymbol = symbol;
    }
}
