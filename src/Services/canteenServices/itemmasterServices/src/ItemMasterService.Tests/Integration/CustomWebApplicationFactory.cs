using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ItemMasterService.Domain.Interfaces;
using ItemMasterService.Infrastructure.Persistence.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ItemMasterService.Tests.Integration;

/// <summary>
/// In-process test server that replaces SQL Server → EF InMemory,
/// removes RabbitMQ consumers, and stubs out Blob storage.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestJwtKey = "SuperSecretKeyForIntegrationTesting_AtLeast32Chars!";
    public const string TestJwtIssuer = "ItemMasterService";
    public const string TestJwtAudience = "ItemMasterServiceClients";

    // Each factory instance gets its own isolated in-memory DB.
    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Tell Program.cs to skip MigrateAsync / SeedAsync
        builder.UseEnvironment("Testing");

        // Override configuration: use known JWT key and disable external dependencies
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"]               = TestJwtKey,
                ["Jwt:Issuer"]            = TestJwtIssuer,
                ["Jwt:Audience"]          = TestJwtAudience,
                ["Jwt:ExpiresInMinutes"]  = "60",
                // Fake connection string – never opened by InMemory EF or in-test calls
                ["ConnectionStrings:DefaultConnection"] = "Server=.;Database=TestPlaceholder;Integrated Security=True;",
                // Disable Blob storage (no Azurite required)
                ["BlobStorage:ConnectionString"] = "UseDevelopmentStorage=false",
                ["BlobStorage:ContainerName"]    = "test-container",
            });
        });

        builder.ConfigureServices(services =>
        {
            // ── Replace SQL Server DbContext with InMemory ────────────────
            services.RemoveAll<DbContextOptions<ItemMasterDbContext>>();
            services.RemoveAll<ItemMasterDbContext>();

            services.AddDbContext<ItemMasterDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));

            // ── Remove all health checks (registered via IConfigureOptions) ──
            // AddCheck<T>() registers checks via IConfigureOptions<HealthCheckServiceOptions>,
            // not as HealthCheckRegistration descriptors.
            services.RemoveAll(typeof(IConfigureOptions<HealthCheckServiceOptions>));

            // Register a simple always-healthy check so /health endpoints work
            services.AddHealthChecks()
                .AddCheck("ready", () => HealthCheckResult.Healthy("Test environment is ready."),
                    tags: new[] { "db", "ready" });

            // ── Remove all background/hosted services (RabbitMQ consumers) ─
            var hostedServices = services
                .Where(d => d.ServiceType == typeof(IHostedService))
                .ToList();
            foreach (var d in hostedServices) services.Remove(d);

            // ── Replace IMessagePublisher with a no-op ───────────────────
            services.RemoveAll<IMessagePublisher>();
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();

            // ── Replace IBlobStorageService with a no-op ─────────────────
            services.RemoveAll<IBlobStorageService>();
            services.AddSingleton<IBlobStorageService, NoOpBlobStorageService>();

            // ── Override JWT token validation to use the test key ─────────
            // Program.cs captures the JWT key at registration time, so we
            // use PostConfigure to override validation parameters afterwards.
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                var testKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtKey));
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = TestJwtIssuer,
                    ValidAudience            = TestJwtAudience,
                    IssuerSigningKey         = testKey,
                    ClockSkew                = TimeSpan.Zero
                };
            });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Ensure the schema exists in the in-memory database
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ItemMasterDbContext>();
        db.Database.EnsureCreated();

        return host;
    }

    // ── JWT helper ────────────────────────────────────────────────────────────

    public string GenerateBearerToken(string username = "testuser", string role = "CanteenUser")
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:   TestJwtIssuer,
            audience: TestJwtAudience,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public HttpClient CreateAuthenticatedClient(string username = "testuser")
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(username));
        return client;
    }
}

// ── Stub implementations ──────────────────────────────────────────────────────

internal sealed class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
        => Task.CompletedTask;
}

internal sealed class NoOpBlobStorageService : IBlobStorageService
{
    public Task<string> UploadItemImageAsync(long itemCode, Stream imageStream, string contentType, CancellationToken ct = default)
        => Task.FromResult($"https://stub/item-{itemCode}.jpg");

    public Task<Stream?> DownloadItemImageAsync(long itemCode, CancellationToken ct = default)
        => Task.FromResult<Stream?>(null);

    public Task DeleteItemImageAsync(long itemCode, CancellationToken ct = default)
        => Task.CompletedTask;
}
