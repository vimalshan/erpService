using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TdsService.Application.Common.Interfaces;
using TdsService.Infrastructure.Persistence;

namespace TdsService.Integration.Tests;

/// <summary>
/// WebApplicationFactory that wires up an in-memory database and a no-op
/// message publisher so integration tests run without SQL Server or RabbitMQ.
/// </summary>
public sealed class TdsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // A dedicated internal service provider that ONLY has the InMemory EF provider.
    // Shared across all DbContext instances created by this factory so EF's internal
    // service provider cache never sees the SQL Server provider.
    private static readonly IServiceProvider InMemoryEfServiceProvider =
        new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // ── Replace SQL Server DbContext with InMemory ─────────────────
            var toRemove = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<TdsDbContext>)
                         || d.ServiceType == typeof(TdsDbContext))
                .ToList();
            foreach (var d in toRemove)
                services.Remove(d);

            // UseInternalServiceProvider supplies an isolated EF service provider
            // that contains ONLY the InMemory database provider, preventing the
            // "multiple database providers registered" error that occurs because
            // AddEntityFrameworkSqlServer services remain in the DI container.
            services.AddDbContext<TdsDbContext>(options =>
                options.UseInMemoryDatabase("TdsIntegrationTests")
                       .UseInternalServiceProvider(InMemoryEfServiceProvider));

            // ── Replace RabbitMQ publisher with no-op ──────────────────
            var mqDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(IMessagePublisher));
            if (mqDescriptor is not null)
                services.Remove(mqDescriptor);

            services.AddScoped<IMessagePublisher, NullMessagePublisher>();
        });
    }

    public async Task InitializeAsync()
    {
        // Ensure the InMemory schema exists before tests run.
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TdsDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    public new Task DisposeAsync() => Task.CompletedTask;
}

/// <summary>No-op message publisher used in integration tests.</summary>
internal sealed class NullMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message,
        CancellationToken ct = default)
        => Task.CompletedTask;
}
