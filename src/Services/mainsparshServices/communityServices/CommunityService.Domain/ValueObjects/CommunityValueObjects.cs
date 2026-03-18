namespace CommunityService.Domain.ValueObjects;

public record CommunityCode(string Value)
{
    public static CommunityCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Community code cannot be empty.", nameof(code));
        
        if (code.Length > 50)
            throw new ArgumentException("Community code cannot exceed 50 characters.", nameof(code));
        
        return new CommunityCode(code.ToUpper());
    }
}

public record CommunityName(string Value)
{
    public static CommunityName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Community name cannot be empty.", nameof(name));
        
        if (name.Length > 255)
            throw new ArgumentException("Community name cannot exceed 255 characters.", nameof(name));
        
        return new CommunityName(name);
    }
}

public record PrivacyLevel(string Value)
{
    private static readonly string[] ValidLevels = { "PUBLIC", "PRIVATE", "RESTRICTED" };

    public static PrivacyLevel Create(string level)
    {
        if (!ValidLevels.Contains(level.ToUpper()))
            throw new ArgumentException($"Invalid privacy level. Must be one of: {string.Join(", ", ValidLevels)}", nameof(level));
        
        return new PrivacyLevel(level.ToUpper());
    }
}

public record CommunityType(string Value)
{
    private static readonly string[] ValidTypes = { "FORUM", "INTEREST_GROUP", "TEAM", "DEPARTMENT" };

    public static CommunityType Create(string type)
    {
        if (!ValidTypes.Contains(type.ToUpper()))
            throw new ArgumentException($"Invalid community type. Must be one of: {string.Join(", ", ValidTypes)}", nameof(type));
        
        return new CommunityType(type.ToUpper());
    }
}

public record CommunityStatus(string Value)
{
    private static readonly string[] ValidStatuses = { "ACTIVE", "INACTIVE", "ARCHIVED" };

    public static CommunityStatus Create(string status)
    {
        if (!ValidStatuses.Contains(status.ToUpper()))
            throw new ArgumentException($"Invalid community status. Must be one of: {string.Join(", ", ValidStatuses)}", nameof(status));
        
        return new CommunityStatus(status.ToUpper());
    }
}

public record MemberRole(string Value)
{
    private static readonly string[] ValidRoles = { "ADMIN", "MODERATOR", "MEMBER", "GUEST" };

    public static MemberRole Create(string role)
    {
        if (!ValidRoles.Contains(role.ToUpper()))
            throw new ArgumentException($"Invalid member role. Must be one of: {string.Join(", ", ValidRoles)}", nameof(role));
        
        return new MemberRole(role.ToUpper());
    }
}

public record MemberStatus(string Value)
{
    private static readonly string[] ValidStatuses = { "ACTIVE", "INACTIVE", "SUSPENDED", "REMOVED" };

    public static MemberStatus Create(string status)
    {
        if (!ValidStatuses.Contains(status.ToUpper()))
            throw new ArgumentException($"Invalid member status. Must be one of: {string.Join(", ", ValidStatuses)}", nameof(status));
        
        return new MemberStatus(status.ToUpper());
    }
}
