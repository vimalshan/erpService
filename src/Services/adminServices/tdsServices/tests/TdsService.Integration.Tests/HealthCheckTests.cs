using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using TdsService.Infrastructure.Persistence;

namespace TdsService.Integration.Tests;

/// <summary>
/// Integration tests for health check endpoints — no auth required.
/// </summary>
public sealed class HealthCheckTests(TdsApiFactory factory)
    : IClassFixture<TdsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_Health_Returns200()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_HealthReady_Returns200_WhenDbIsAvailable()
    {
        // Pre-warm the DB scope so the health check passes.
        using var scope = factory.Services.CreateScope();
        _ = scope.ServiceProvider.GetRequiredService<TdsDbContext>();

        var response = await _client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
