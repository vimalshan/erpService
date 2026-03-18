using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to PF_SUB_ACCOUNT table.</summary>
public class PfSubAccount : BaseEntity
{
    public long SubAccCod { get; private set; }
    public string SubAccNam { get; private set; } = default!;

    private PfSubAccount() { }

    public static PfSubAccount Create(long code, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Sub account name cannot be empty.", nameof(name));

        return new PfSubAccount { SubAccCod = code, SubAccNam = name };
    }
}
