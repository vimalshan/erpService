using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DocumentService.Domain.Entities;

namespace DocumentService.Infrastructure.Data.Seed;

public class DatabaseSeeder
{
    private readonly DocumentDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(DocumentDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);

        if (!await _context.LoanDocuments.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Seeding initial loan documents...");

            var documents = new[]
            {
                LoanDocument.Create(1001, 1, 1, 100),
                LoanDocument.Create(1002, 1, 2, 100),
                LoanDocument.Create(1003, 2, 1, 101),
            };

            foreach (var doc in documents)
                doc.ClearDomainEvents();

            await _context.LoanDocuments.AddRangeAsync(documents, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seed data inserted: {Count} loan document(s).", documents.Length);
        }
    }
}
