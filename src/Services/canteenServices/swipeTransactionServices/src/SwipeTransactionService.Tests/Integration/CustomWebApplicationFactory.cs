using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SwipeTransactionService.Domain.Interfaces.Repositories;
using SwipeTransactionService.Infrastructure.Dapper;
using SwipeTransactionService.Infrastructure.Persistence;

namespace SwipeTransactionService.Tests.Integration;

/// <summary>
/// In-process test server: replaces SQL Server → EF InMemory,
/// stubs Dapper query service, removes RabbitMQ health check, overrides JWT.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // ── Use the SAME secret as appsettings.json so the AuthController tokens also validate ──
    public const string TestJwtSecret   = "SwipeTransactionService_SuperSecretKey_32chars!!";
    public const string TestJwtIssuer   = "SwipeTransactionService";
    public const string TestJwtAudience = "SwipeTransactionServiceClients";

    private readonly string _dbName = $"SwipeTestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // ── Replace SQL Server EF with InMemory ───────────────────────
            // Must remove BOTH the options object AND the internal
            // IDbContextOptionsConfiguration<> entries (which carry the SQL Server
            // provider registration). The interface is internal to EF Core, so we
            // locate its descriptors by checking generic type arguments via reflection.
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<SwipeTransactionDbContext>) ||
                    d.ServiceType == typeof(SwipeTransactionDbContext) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.Name.StartsWith("IDbContextOptionsConfiguration") &&
                     d.ServiceType.GetGenericArguments().Contains(typeof(SwipeTransactionDbContext))))
                .ToList();
            foreach (var d in toRemove) services.Remove(d);

            services.AddDbContext<SwipeTransactionDbContext>(opts =>
                opts.UseInMemoryDatabase(_dbName));

            // ── Remove all health check registrations ─────────────────────
            // (DbContextCheck + RabbitMQ check both need real connections)
            services.RemoveAll(typeof(IConfigureOptions<HealthCheckServiceOptions>));
            services.AddHealthChecks()
                .AddCheck("ready",
                    () => HealthCheckResult.Healthy("Test environment is healthy."),
                    tags: new[] { "db", "ready" });

            // ── Replace EF-backed swipe repository with in-memory stub ────
            // SwipeCardUpload has HasNoKey() — EF InMemory cannot track/add it.
            // The stub stores entities in a plain list, bypassing EF entirely.
            services.RemoveAll<ISwipeCardUploadRepository>();
            services.AddSingleton<ISwipeCardUploadRepository, InMemorySwipeCardUploadRepository>();

            // ── Override Dapper query service with a dummy conn-string instance ─
            // SwipeReportQueryService is sealed, so we replace with a new instance
            // using a no-op connection string. Don't call GetSummaryByBatch /
            // GetDailyAvailed in tests as those would try to open a real SQL connection.
            services.RemoveAll<SwipeReportQueryService>();
            services.AddSingleton(new SwipeReportQueryService("Server=test;Database=NoOpSwipe;"));

            // ── Override JWT Bearer validation to use the test key ────────
            // (JwtTokenService is already registered with the right key via config)
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme, opts =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret));
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = TestJwtIssuer,
                        ValidAudience            = TestJwtAudience,
                        IssuerSigningKey         = key,
                        ClockSkew                = TimeSpan.Zero
                    };
                });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Create the in-memory schema
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SwipeTransactionDbContext>();
        db.Database.EnsureCreated();

        return host;
    }

    // ── JWT helper ────────────────────────────────────────────────────────

    public string GenerateBearerToken(string userId = "1", string username = "testuser",
        string[] roles = null!)
    {
        roles ??= new[] { "Admin", "CanteenManager" };
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,  userId),
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer:             TestJwtIssuer,
            audience:           TestJwtAudience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", GenerateBearerToken());
        return client;
    }
}
