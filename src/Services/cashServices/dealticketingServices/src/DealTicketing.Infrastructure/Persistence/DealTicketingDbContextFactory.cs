using System.IO;

using Microsoft.Extensions.Configuration;
using DealTicketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MediatR;

namespace DealTicketing.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for EF Core migrations.
/// Used by `dotnet ef migrations add` without needing the running application.
/// </summary>
public class DealTicketingDbContextFactory : IDesignTimeDbContextFactory<DealTicketingDbContext>
{
    public DealTicketingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DealTicketingDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CASHDB;Integrated Security=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString,
            sql => sql.MigrationsAssembly(typeof(DealTicketingDbContextFactory).Assembly.FullName));

        return new DealTicketingDbContext(optionsBuilder.Options, new NoopMediator());
    }

    private class NoopMediator : IMediator
    {
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => Task.FromResult(default(TResponse)!);
        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest => Task.CompletedTask;
        public Task<object?> Send(object request, CancellationToken cancellationToken = default) => Task.FromResult<object?>(null);
    }
}
