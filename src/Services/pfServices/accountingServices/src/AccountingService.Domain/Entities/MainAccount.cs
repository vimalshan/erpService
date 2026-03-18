using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to MAINACCOUNT_MASTER table – chart of accounts.</summary>
public class MainAccount : BaseEntity
{
    public string MainAccountCode { get; private set; } = default!;
    public string? MainAccountName { get; private set; }
    public string? MainAccountShrtName { get; private set; }
    public string? MainClosureFlag { get; private set; }

    private MainAccount() { }

    public static MainAccount Create(string code, string? name, string? shortName, string? closureFlag = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Account code cannot be empty.", nameof(code));

        return new MainAccount
        {
            MainAccountCode = code,
            MainAccountName = name,
            MainAccountShrtName = shortName,
            MainClosureFlag = closureFlag ?? "N"
        };
    }

    public void UpdateName(string name, string? shortName)
    {
        MainAccountName = name;
        MainAccountShrtName = shortName;
    }

    public void Close() => MainClosureFlag = "Y";
    public bool IsClosed => MainClosureFlag == "Y";
}
