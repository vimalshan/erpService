namespace WebsiteContentService.Domain.ValueObjects;

using WebsiteContentService.Domain.Common;

public class PageCode : ValueObject
{
    public string Value { get; }

    private PageCode(string value) => Value = value;

    public static PageCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Page code cannot be empty.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Page code cannot exceed 100 characters.", nameof(value));

        return new PageCode(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
