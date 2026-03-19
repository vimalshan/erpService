namespace InvoiceProcessing.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDocumentRepository Documents { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
