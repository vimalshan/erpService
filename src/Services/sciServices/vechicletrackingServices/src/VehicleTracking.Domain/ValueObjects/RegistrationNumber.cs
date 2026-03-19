namespace VehicleTracking.Domain.ValueObjects;

public record RegistrationNumber
{
    public string Part1 { get; init; } = string.Empty;
    public string? Part2 { get; init; }
    public string? Part3 { get; init; }
    public string Part4 { get; init; } = string.Empty;

    public static RegistrationNumber Create(string part1, string? part2, string? part3, string part4)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(part1);
        ArgumentException.ThrowIfNullOrWhiteSpace(part4);

        return new RegistrationNumber
        {
            Part1 = part1,
            Part2 = part2,
            Part3 = part3,
            Part4 = part4
        };
    }

    public override string ToString() => $"{Part1}-{Part2}-{Part3}-{Part4}";
}
