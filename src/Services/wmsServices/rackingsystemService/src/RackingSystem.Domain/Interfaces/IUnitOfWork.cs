namespace RackingSystem.Domain.Interfaces;

public interface IUnitOfWork
{
    IRackRepository Racks { get; }
    IShelfRepository Shelves { get; }
    IBinRepository Bins { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
