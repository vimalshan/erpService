using Microsoft.Extensions.Logging;

namespace AuthProvider.Infrastructure.Adapters;

/// <summary>
/// Adapter pattern – wraps an external identity provider (e.g., Azure AD, Auth0)
/// and converts its response model to the internal domain model.
/// </summary>
public sealed class ExternalAuthAdapter
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalAuthAdapter> _logger;

    public ExternalAuthAdapter(HttpClient httpClient, ILogger<ExternalAuthAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>Validate an external provider token and return the internal user claim set.</summary>
    public async Task<ExternalUserInfo?> ValidateExternalTokenAsync(
        string provider, string token, CancellationToken ct = default)
    {
        _logger.LogInformation("Validating external token from provider={Provider}", provider);

        // Adapter: translate external provider format → internal ExternalUserInfo model
        var url = provider.ToLowerInvariant() switch
        {
            "google" => $"https://oauth2.googleapis.com/tokeninfo?id_token={Uri.EscapeDataString(token)}",
            _ => throw new NotSupportedException($"External provider '{provider}' is not supported.")
        };

        var response = await _httpClient.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("External token validation failed: {StatusCode}", response.StatusCode);
            return null;
        }

        var content = await response.Content.ReadAsStringAsync(ct);
        // In production parse the real provider response; simplified here
        return new ExternalUserInfo
        {
            Provider = provider,
            ExternalId = "external-id-placeholder",
            Email = "user@example.com",
            DisplayName = "External User"
        };
    }
}

public sealed class ExternalUserInfo
{
    public string Provider { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
