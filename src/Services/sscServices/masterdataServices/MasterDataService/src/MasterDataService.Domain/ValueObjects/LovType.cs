namespace MasterDataService.Domain.ValueObjects;

public sealed record LovType
{
    public string Code { get; }
    public string Name { get; }

    private LovType(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public static LovType Create(string code, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (code.Length > 10)
            throw new ArgumentException("LOV Type code cannot exceed 10 characters.", nameof(code));
        if (name.Length > 50)
            throw new ArgumentException("LOV Type name cannot exceed 50 characters.", nameof(name));

        return new LovType(code, name);
    }
}
