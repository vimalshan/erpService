namespace TimeSheetService.Domain.ValueObjects;

/// <summary>Time Entry Type: S=Self, M=Manual, A=Automatic</summary>
public sealed class EntryType : IEquatable<EntryType>
{
    public static readonly EntryType Self       = new('S', "Self");
    public static readonly EntryType Manual     = new('M', "Manual");
    public static readonly EntryType Automatic  = new('A', "Automatic");

    public char Code { get; }
    public string Name { get; }

    private EntryType(char code, string name) { Code = code; Name = name; }

    public static EntryType FromCode(char code) => code switch
    {
        'S' => Self,
        'M' => Manual,
        'A' => Automatic,
        _   => throw new ArgumentException($"Invalid entry type code: {code}")
    };

    public static IEnumerable<EntryType> GetAll() => [Self, Manual, Automatic];

    public bool Equals(EntryType? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is EntryType et && Equals(et);
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => Name;

    public static bool operator ==(EntryType? l, EntryType? r) => l is null ? r is null : l.Equals(r);
    public static bool operator !=(EntryType? l, EntryType? r) => !(l == r);
}
