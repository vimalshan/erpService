namespace AdminService.Domain.ValueObjects;

public record LocationStatus
{
    public char Value { get; }

    public static readonly LocationStatus Active = new('A');
    public static readonly LocationStatus Inactive = new('I');

    private LocationStatus(char value) => Value = value;

    public static LocationStatus From(char value)
    {
        return value switch
        {
            'A' or 'I' => new LocationStatus(value),
            _ => throw new ArgumentException($"Invalid location status: {value}. Valid values: A, I")
        };
    }

    public override string ToString() => Value.ToString();
}
