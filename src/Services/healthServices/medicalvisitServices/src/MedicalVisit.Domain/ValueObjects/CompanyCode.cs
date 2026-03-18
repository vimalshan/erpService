using MedicalVisit.Domain.Common;

namespace MedicalVisit.Domain.ValueObjects;

public class CompanyCode : ValueObject
{
    public string Value { get; private set; }

    private CompanyCode(string value)
    {
        Value = value;
    }

    public static CompanyCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Company code cannot be empty", nameof(value));

        if (value.Length > 3)
            throw new ArgumentException("Company code cannot exceed 3 characters", nameof(value));

        return new CompanyCode(value.Trim().ToUpper());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CompanyCode companyCode) => companyCode.Value;
}
