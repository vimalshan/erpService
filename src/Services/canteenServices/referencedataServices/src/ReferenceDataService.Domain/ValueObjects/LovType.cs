namespace ReferenceDataService.Domain.ValueObjects;

public record LovType
{
    public string Code { get; init; } = string.Empty;

    public LovType(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 3)
            throw new ArgumentException("LOV Type code must be 1-3 characters.", nameof(code));

        Code = code.Trim();
    }

    private LovType() { }
}
