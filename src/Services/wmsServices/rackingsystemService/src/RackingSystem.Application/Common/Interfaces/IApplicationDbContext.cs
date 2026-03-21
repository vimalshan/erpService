using RackingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RackingSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Rack> Racks { get; }
    DbSet<Shelf> Shelves { get; }
    DbSet<Bin> Bins { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
