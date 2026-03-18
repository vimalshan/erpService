using EximManagement.Domain.Common;

namespace EximManagement.Domain.ValueObjects;

/// <summary>Value object representing an HS/ITC code.</summary>
public class HsCode : ValueObject
{
    public long Code { get; }
    public string? Chapter => Code.ToString().Length >= 2 ? Code.ToString()[..2] : null;
    public string? Heading => Code.ToString().Length >= 4 ? Code.ToString()[..4] : null;

    private HsCode(long code) => Code = code;

    public static HsCode Create(long code)
    {
        if (code <= 0) throw new ArgumentException("HS Code must be positive.", nameof(code));
        return new HsCode(code);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code.ToString();
}
