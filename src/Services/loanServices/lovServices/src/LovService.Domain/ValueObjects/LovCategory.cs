namespace LovService.Domain.ValueObjects;

/// <summary>
/// LOV Category: F = Fixed, V = Variable
/// </summary>
public sealed class LovCategory
{
    public static readonly LovCategory Fixed = new('F');
    public static readonly LovCategory Variable = new('V');

    public char Value { get; }

    private LovCategory(char value) => Value = value;

    public static LovCategory From(char value) => value switch
    {
        'F' => Fixed,
        'V' => Variable,
        _ => throw new ArgumentException($"Invalid LOV Category '{value}'. Valid values are F (Fixed) and V (Variable).")
    };

    public override string ToString() => Value.ToString();

    public override bool Equals(object? obj) => obj is LovCategory other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
