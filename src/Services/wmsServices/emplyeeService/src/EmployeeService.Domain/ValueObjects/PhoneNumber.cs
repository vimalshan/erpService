using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber? Create(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        if (phone.Length > 20)
            throw new ArgumentException("Phone number cannot exceed 20 characters.");

        return new PhoneNumber(phone);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
