namespace MenuAndSecurityService.Domain.ValueObjects;

public sealed record MenuPage
{
    public string PageName { get; }

    public MenuPage(string pageName)
    {
        if (string.IsNullOrWhiteSpace(pageName))
            throw new ArgumentException("Page name cannot be empty.", nameof(pageName));

        if (pageName.Length > 200)
            throw new ArgumentException("Page name cannot exceed 200 characters.", nameof(pageName));

        PageName = pageName;
    }
}
