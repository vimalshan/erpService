namespace FinanceService.Domain.Enums;

public enum AccountType
{
    Advance,
    Plan,
    Settlement
}

public static class AccountTypeExtensions
{
    public static string ToCode(this AccountType type) => type switch
    {
        AccountType.Advance => "ADV",
        AccountType.Plan => "PLN",
        AccountType.Settlement => "SET",
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static AccountType FromCode(string code) => code switch
    {
        "ADV" => AccountType.Advance,
        "PLN" => AccountType.Plan,
        "SET" => AccountType.Settlement,
        _ => throw new ArgumentOutOfRangeException(nameof(code))
    };
}
