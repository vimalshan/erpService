using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.ValueObjects;

public class CoverageType : ValueObject
{
    public const string Employee = "EMPLOYEE";
    public const string Family = "FAMILY";
    public const string Dependent = "DEPENDENT";

    public string Value { get; }

    private CoverageType(string value)
    {
        Value = value;
    }

    public static CoverageType Employee_Coverage => new(Employee);
    public static CoverageType Family_Coverage => new(Family);
    public static CoverageType Dependent_Coverage => new(Dependent);

    public static CoverageType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Coverage type cannot be empty", nameof(value));

        return new CoverageType(value.ToUpper());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
