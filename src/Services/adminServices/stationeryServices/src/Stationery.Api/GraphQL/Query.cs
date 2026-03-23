using Microsoft.EntityFrameworkCore;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;
using Stationery.Infrastructure.Persistence;
using HotChocolate.Types;
using HotChocolate.Data;

namespace Stationery.Api.GraphQL;

public class Query
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StationaryMaster> GetStationaryItems([Service] StationeryDbContext db)
        => db.Set<StationaryMaster>();

    public async Task<StationaryMaster?> GetStationaryItem(long id, [Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<StationaryMaster>().GetByIdAsync(id);

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<RequestMain> GetRequest(long id, [Service] StationeryDbContext db)
        => db.Set<RequestMain>().Where(r => r.Id == id);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<RequestMain> GetRequests([Service] StationeryDbContext db)
        => db.Set<RequestMain>();

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<OrderMain> GetOrder(long id, [Service] StationeryDbContext db)
        => db.Set<OrderMain>().Where(o => o.Id == id);

    public async Task<IEnumerable<StationeryReorderAlert>> GetReorderAlerts([Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<StationeryReorderAlert>().FindAsync(a => a.Resolved == "N");
}
