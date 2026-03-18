namespace TeamServices.Domain.ValueObjects;

using TeamServices.Domain.Common;

public class GradeCategory : ValueObject
{
    public char Value { get; private set; }

    private GradeCategory() { }

    public GradeCategory(char value)
    {
        if (!char.IsLetterOrDigit(value))
            throw new ArgumentException("Grade category must be a letter or digit.");

        Value = char.ToUpperInvariant(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
