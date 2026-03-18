namespace AccessService.Domain.ValueObjects;

/// <summary>
/// Value Object for role types
/// </summary>
public class RoleType : IEquatable<RoleType>
{
    private static readonly Dictionary<char, RoleType> AllowedTypes = new()
    {
        { 'S', new RoleType('S', "Super User") },
        { 'U', new RoleType('U', "Unit Access") },
        { 'C', new RoleType('C', "Calendar Access") }
    };

    public char Value { get; }
    
    public string Description { get; }

    private RoleType(char value, string description)
    {
        Value = value;
        Description = description;
    }

    public static RoleType CreateFrom(char value)
    {
        if (!AllowedTypes.TryGetValue(char.ToUpper(value), out var roleType))
            throw new InvalidOperationException($"Invalid role type: {value}");

        return roleType;
    }

    public static RoleType SuperUser => AllowedTypes['S'];
    public static RoleType UnitAccess => AllowedTypes['U'];
    public static RoleType CalendarAccess => AllowedTypes['C'];

    public override bool Equals(object? obj)
    {
        return Equals(obj as RoleType);
    }

    public bool Equals(RoleType? other)
    {
        return other?.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Value} - {Description}";
    }

    public static bool operator ==(RoleType? left, RoleType? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(RoleType? left, RoleType? right)
    {
        return !(left == right);
    }
}
