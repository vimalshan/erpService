namespace MasterService.Domain.ValueObjects;

public sealed record SkillCode
{
    public long Value { get; }

    public SkillCode(long value)
    {
        if (value <= 0) throw new ArgumentException("Skill code must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator long(SkillCode code) => code.Value;
    public static implicit operator SkillCode(long value) => new(value);
    public override string ToString() => Value.ToString();
}
