using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.Forex;

public class ForexDetail : Entity<string>
{
    public string ForexRequestId { get; private set; } = string.Empty;
    public decimal SourceCurrencyValue { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal ForexValue { get; private set; }
    public decimal? ExchangeRate { get; private set; }
    public decimal? ExchangeValue { get; private set; }
    public string? PayMode { get; private set; }

    protected ForexDetail() { }

    public static ForexDetail Create(
        string id, string forexRequestId, decimal srcCurrencyValue,
        string currency, decimal forexValue, decimal? exchangeRate = null,
        decimal? exchangeValue = null, string? payMode = null)
        => new()
        {
            Id = id,
            ForexRequestId = forexRequestId,
            SourceCurrencyValue = srcCurrencyValue,
            Currency = currency,
            ForexValue = forexValue,
            ExchangeRate = exchangeRate,
            ExchangeValue = exchangeValue,
            PayMode = payMode
        };
}
