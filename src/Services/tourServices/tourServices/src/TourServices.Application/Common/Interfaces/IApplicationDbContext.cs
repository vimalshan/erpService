using TourServices.Domain.Aggregates;
using TourServices.Domain.Entities;

namespace TourServices.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    System.Data.Common.DbConnection GetDbConnection();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
