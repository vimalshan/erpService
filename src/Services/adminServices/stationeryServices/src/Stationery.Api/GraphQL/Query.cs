using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;
using HotChocolate.Types;
using HotChocolate.Data;

namespace Stationery.Api.GraphQL;

public class Query
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<StationaryMaster>> GetStationaryItems([Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<StationaryMaster>().GetAllAsync();

    public async Task<StationaryMaster?> GetStationaryItem(long id, [Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<StationaryMaster>().GetByIdAsync(id);

    public async Task<RequestMain?> GetRequest(long id, [Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<RequestMain>().GetByIdAsync(id);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<RequestMain>> GetRequests([Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<RequestMain>().GetAllAsync();

    public async Task<OrderMain?> GetOrder(long id, [Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<OrderMain>().GetByIdAsync(id);

    public async Task<IEnumerable<StationeryReorderAlert>> GetReorderAlerts([Service] IUnitOfWork unitOfWork)
        => await unitOfWork.Repository<StationeryReorderAlert>().FindAsync(a => a.Resolved == 'N');
}
