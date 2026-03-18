namespace CurrencyManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Currency
/// </summary>
public class CurrencyDto
{
    public long CurrencyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// Data transfer object for Exchange Rate
/// </summary>
public class ExchangeRateDto
{
    public long RateId { get; set; }
    public long FinancialYear { get; set; }
    public long Month { get; set; }
    public long FromCurrencyId { get; set; }
    public long ToCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// Data transfer object for Organization Currency Mapping
/// </summary>
public class OrganizationCurrencyDto
{
    public long OrganizationId { get; set; }
    public long CurrencyId { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// Data transfer object for currency conversion result
/// </summary>
public class ConvertedAmountDto
{
    public decimal OriginalAmount { get; set; }
    public long FromCurrencyId { get; set; }
    public long ToCurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal ConvertedAmount { get; set; }
    public long FinancialYear { get; set; }
    public long Month { get; set; }
}
