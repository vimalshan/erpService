namespace WebsiteContentService.Domain.ValueObjects;

using WebsiteContentService.Domain.Common;

public class PublishStatus : ValueObject
{
    public string Value { get; }

    private PublishStatus(string value) => Value = value;

    public static PublishStatus Draft => new("DRAFT");
    public static PublishStatus Published => new("PUBLISHED");
    public static PublishStatus Archived => new("ARCHIVED");
    public static PublishStatus Active => new("ACTIVE");
    public static PublishStatus Inactive => new("INACTIVE");

    public static PublishStatus Create(string value)
    {
        var allowed = new[] { "DRAFT", "PUBLISHED", "ARCHIVED", "ACTIVE", "INACTIVE" };
        if (!allowed.Contains(value, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid status: '{value}'. Allowed: {string.Join(", ", allowed)}");

        return new PublishStatus(value.ToUpperInvariant());
    }

    public bool IsDraft => Value == "DRAFT";
    public bool IsPublished => Value == "PUBLISHED";
    public bool IsArchived => Value == "ARCHIVED";
    public bool IsActive => Value == "ACTIVE";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
