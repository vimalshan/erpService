using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.ValueObjects;

public class CompanyCode : ValueObject
{
    public string Value { get; private set; } = null!;

    private CompanyCode() { }

    public CompanyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Company code must be 1-3 characters.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CompanyCode code) => code.Value;
}
