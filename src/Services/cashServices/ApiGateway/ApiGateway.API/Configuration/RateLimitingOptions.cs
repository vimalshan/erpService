namespace ApiGateway.API.Configuration;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";

    public int PermitLimit { get; set; } = 100;
    public int WindowSeconds { get; set; } = 60;
    public int QueueLimit { get; set; } = 10;
    public int TokenPermitLimit { get; set; } = 20;
    public int TokenReplenishSeconds { get; set; } = 10;
}
