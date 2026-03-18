namespace Shared.Infrastructure.Utilities;

using System.Text.RegularExpressions;

/// <summary>
/// Validation utilities for common business rules
/// </summary>
public static class ValidationUtilities
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
        var phoneRegex = new Regex(@"^[0-9\-\+\s\(\)]{7,}$");
        return phoneRegex.IsMatch(phoneNumber);
    }

    public static bool IsValidEmployeeNumber(string employeeNumber)
    {
        if (string.IsNullOrWhiteSpace(employeeNumber)) return false;
        var empRegex = new Regex(@"^[A-Za-z0-9]{4,12}$");
        return empRegex.IsMatch(employeeNumber);
    }

    public static bool IsValidDate(DateTime date)
    {
        return date != DateTime.MinValue && date <= DateTime.UtcNow;
    }

    public static bool IsValidFutureDate(DateTime date)
    {
        return date > DateTime.UtcNow;
    }

    public static bool IsValidPastDate(DateTime date)
    {
        return date < DateTime.UtcNow;
    }

    public static bool IsValidAlphanumeric(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        var alphanumericRegex = new Regex(@"^[a-zA-Z0-9_-]+$");
        return alphanumericRegex.IsMatch(text);
    }

    public static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    public static bool IsValidPercentage(decimal value)
    {
        return value >= 0 && value <= 100;
    }

    public static bool IsValidQuantity(int quantity)
    {
        return quantity > 0;
    }

    public static bool IsValidPrice(decimal price)
    {
        return price > 0 && price <= decimal.MaxValue;
    }
}

/// <summary>
/// String manipulation utilities
/// </summary>
public static class StringUtilities
{
    public static string Truncate(string? text, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        if (text.Length <= maxLength) return text;
        return text.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static string ToTitleCase(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.TextInfo.ToTitleCase(text.ToLower());
    }

    public static string ToKebabCase(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return Regex.Replace(text, "(?<!^)([A-Z])", "-$1").ToLower();
    }

    public static string ToSnakeCase(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return Regex.Replace(text, "(?<!^)([A-Z])", "_$1").ToLower();
    }

    public static string ToCamelCase(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return char.ToLowerInvariant(text[0]) + text.Substring(1);
    }

    public static string GenerateSlug(string? text, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        // Convert to lowercase and remove accents
        var normalizedText = text.Normalize(System.Text.NormalizationForm.FormD);
        var chars = normalizedText.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) 
            != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
        var cleanText = new string(chars).Normalize(System.Text.NormalizationForm.FormC);

        // Replace spaces with hyphens and remove special characters
        var slug = Regex.Replace(cleanText.ToString(), @"[^a-z0-9\s-]", string.Empty).ToLower();
        slug = Regex.Replace(slug, @"\s+", "-").ToLower();

        return slug.Length > maxLength ? slug.Substring(0, maxLength).TrimEnd('-') : slug;
    }

    public static string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
        var parts = email.Split('@');
        var name = parts[0];
        var domain = parts[1];

        if (name.Length <= 2) return $"{name[0]}***@{domain}";

        var maskedName = name[0] + new string('*', name.Length - 2) + name[name.Length - 1];
        return $"{maskedName}@{domain}";
    }

    public static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 4) return phoneNumber;
        return new string('*', phoneNumber.Length - 4) + phoneNumber.Substring(phoneNumber.Length - 4);
    }
}

/// <summary>
/// Date/time utilities
/// </summary>
public static class DateTimeUtilities
{
    public static int GetAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    public static string GetTimeSinceUtc(DateTime utcDateTime)
    {
        var timeSpan = DateTime.UtcNow - utcDateTime;

        return timeSpan.TotalSeconds < 60 ? $"{Math.Floor(timeSpan.TotalSeconds)} seconds ago"
            : timeSpan.TotalMinutes < 60 ? $"{Math.Floor(timeSpan.TotalMinutes)} minutes ago"
            : timeSpan.TotalHours < 24 ? $"{Math.Floor(timeSpan.TotalHours)} hours ago"
            : timeSpan.TotalDays < 30 ? $"{Math.Floor(timeSpan.TotalDays)} days ago"
            : timeSpan.TotalDays < 365 ? $"{Math.Floor(timeSpan.TotalDays / 30)} months ago"
            : $"{Math.Floor(timeSpan.TotalDays / 365)} years ago";
    }

    public static bool IsBusinessHours(DateTime dateTime, int startHour = 9, int endHour = 17)
    {
        return dateTime.DayOfWeek != DayOfWeek.Saturday
            && dateTime.DayOfWeek != DayOfWeek.Sunday
            && dateTime.Hour >= startHour
            && dateTime.Hour < endHour;
    }

    public static int GetWeekNumber(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date,
            System.Globalization.CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);
    }
}

/// <summary>
/// Pagination utilities
/// </summary>
public static class PaginationUtilities
{
    public static (int Skip, int Take) GetPaginationValues(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Max page size limit

        var skip = (pageNumber - 1) * pageSize;
        return (skip, pageSize);
    }

    public static int CalculateTotalPages(int totalCount, int pageSize)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static bool IsValidPageNumber(int pageNumber, int totalPages)
    {
        return pageNumber >= 1 && pageNumber <= totalPages;
    }
}

/// <summary>
/// Guid utilities
/// </summary>
public static class GuidUtilities
{
    public static bool TryParseGuid(string? value, out Guid result)
    {
        return Guid.TryParse(value, out result);
    }

    public static Guid? SafeParseGuid(string? value)
    {
        return Guid.TryParse(value, out var result) ? result : null;
    }

    public static bool IsValidGuid(string? value)
    {
        return Guid.TryParse(value, out _);
    }
}
