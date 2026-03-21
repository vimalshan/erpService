namespace AdminService.Domain.ValueObjects;

/// <summary>
/// Represents an admin type value object
/// </summary>
public record AdminType
{
    public const string Travel = "T";
    public const string Stay = "S";
    public const string Meeting = "M";

    /// <summary>
    /// Admin type value
    /// </summary>
    public string Value { get; init; }

    private static readonly string[] ValidTypes = { Travel, Stay, Meeting };

    public AdminType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Admin type cannot be empty", nameof(value));

        if (!ValidTypes.Contains(value.ToUpper()))
            throw new ArgumentException($"Invalid admin type. Valid values: {string.Join(", ", ValidTypes)}", nameof(value));

        Value = value.ToUpper();
    }

    public override string ToString() => Value;

    public bool IsTravel => Value == Travel;
    public bool IsStay => Value == Stay;
    public bool IsMeeting => Value == Meeting;
}
