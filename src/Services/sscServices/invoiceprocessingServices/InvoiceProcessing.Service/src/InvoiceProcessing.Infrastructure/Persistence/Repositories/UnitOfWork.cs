using InvoiceProcessing.Domain.Interfaces;

namespace InvoiceProcessing.Infrastructure.Persistence.Repositories;

public class UnitOfWork(InvoiceProcessingDbContext context) : IUnitOfWork
{
    private IDocumentRepository? _documents;

    public IDocumentRepository Documents => _documents ??= new DocumentRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
