using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to ACC_LOOKUP table – maps contribution types to accounts.</summary>
public class AccountLookup : BaseEntity
{
    public string ConTyp { get; private set; } = default!;
    public string EdCod { get; private set; } = default!;
    public long? AccCod { get; private set; }
    public string? TrnTyp { get; private set; }

    private AccountLookup() { }

    public static AccountLookup Create(string conTyp, string edCod, long? accCod, string? trnTyp)
    {
        return new AccountLookup
        {
            ConTyp = conTyp,
            EdCod = edCod,
            AccCod = accCod,
            TrnTyp = trnTyp
        };
    }
}
