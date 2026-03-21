using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class Currency : AggregateRoot<long>
{
    public string CurrencyCode { get; private set; } = string.Empty;
    public string? CurrencyName { get; private set; }
    public string? CurrencySymbol { get; private set; }

    private Currency() { }

    public static Currency Create(long id, string code, string? name, string? symbol)
    {
        var entity = new Currency
        {
            Id = id,
            CurrencyCode = code,
            CurrencyName = name,
            CurrencySymbol = symbol
        };
        entity.AddDomainEvent(new Events.CurrencyCreatedEvent(id, code));
        return entity;
    }

    public void Update(string code, string? name, string? symbol)
    {
        CurrencyCode = code;
        CurrencyName = name;
        CurrencySymbol = symbol;
    }
}
