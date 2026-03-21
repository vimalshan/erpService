using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.ValueObjects;

public class GlSegments : ValueObject
{
    public string ConcatenatedSegments { get; private set; } = string.Empty;
    public string AccountType { get; private set; } = string.Empty;

    private GlSegments() { }

    public GlSegments(string concatenatedSegments, string accountType)
    {
        ConcatenatedSegments = concatenatedSegments ?? throw new ArgumentNullException(nameof(concatenatedSegments));
        AccountType = accountType ?? throw new ArgumentNullException(nameof(accountType));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ConcatenatedSegments;
        yield return AccountType;
    }
}
