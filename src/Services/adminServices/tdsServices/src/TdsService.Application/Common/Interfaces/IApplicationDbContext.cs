using TdsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TdsService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TdsVendor> TdsVendors { get; }
    DbSet<TdsFile> TdsFiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
