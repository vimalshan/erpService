namespace MasterService.Domain.ValueObjects;

public sealed record SkillType
{
    public static readonly SkillType Technical = new('T');
    public static readonly SkillType Behavioural = new('B');
    public static readonly SkillType Functional = new('F');

    public char Value { get; }

    public SkillType(char value)
    {
        if (!IsValid(value))
            throw new ArgumentException($"Invalid skill type '{value}'. Allowed: T, B, F.", nameof(value));
        Value = char.ToUpper(value);
    }

    private static bool IsValid(char c) => c is 'T' or 't' or 'B' or 'b' or 'F' or 'f';

    public static implicit operator char(SkillType t) => t.Value;
    public override string ToString() => Value.ToString();
}
