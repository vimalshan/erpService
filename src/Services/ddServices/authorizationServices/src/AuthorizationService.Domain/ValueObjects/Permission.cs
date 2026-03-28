namespace AuthorizationService.Domain.ValueObjects;

/// <summary>
/// Permission value object representing a right permission flag
/// </summary>
public class Permission
{
    public char Value { get; set; }

    public Permission(char value)
    {
        if (value != 'Y' && value != 'N' && value != '1' && value != '0')
            throw new ArgumentException("Permission must be Y, N, 1, or 0", nameof(value));
        Value = value;
    }

    public bool IsGranted => Value == 'Y' || Value == '1';

    public override bool Equals(object? obj)
    {
        if (obj is Permission other)
            return Value == other.Value;
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
