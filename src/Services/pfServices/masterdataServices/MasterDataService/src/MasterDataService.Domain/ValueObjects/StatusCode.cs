namespace MasterDataService.Domain.ValueObjects;

public sealed record StatusCode
{
    public string Value { get; init; }

    public StatusCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 2)
            throw new ArgumentException("Status code must be 1-2 characters.");
        Value = value;
    }

    public static implicit operator string(StatusCode code) => code.Value;
    public static implicit operator StatusCode(string value) => new(value);
}
