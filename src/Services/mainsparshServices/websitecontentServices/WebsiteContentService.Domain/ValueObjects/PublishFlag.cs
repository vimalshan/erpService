namespace WebsiteContentService.Domain.ValueObjects;

using WebsiteContentService.Domain.Common;

public class PublishFlag : ValueObject
{
    public char Value { get; }

    private PublishFlag(char value) => Value = value;

    public static PublishFlag Yes => new('Y');
    public static PublishFlag No => new('N');

    public static PublishFlag Create(char value)
    {
        if (value != 'Y' && value != 'N')
            throw new ArgumentException("Publish flag must be 'Y' or 'N'.", nameof(value));

        return new PublishFlag(value);
    }

    public static PublishFlag FromBool(bool value) => value ? Yes : No;

    public bool IsYes => Value == 'Y';
    public bool IsNo => Value == 'N';

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
