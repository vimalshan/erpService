using MasterDataService.Domain.Entities;

namespace MasterDataService.Domain.Interfaces;

public interface IGuestHouseRepository : IRepository<GuestHouse>
{
    Task<GuestHouse?> GetByAdminCodeAsync(long adminCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GuestHouse>> GetWithRoomsAsync(CancellationToken cancellationToken = default);
}

public interface IGuestHouseRoomRepository : IRepository<GuestHouseRoom>
{
    Task<IReadOnlyList<GuestHouseRoom>> GetByGuestHouseCodeAsync(long guestHouseCode, CancellationToken cancellationToken = default);
}

public interface IAreaRepository : IRepository<Area>
{
    Task<Area?> GetByAreaIdAsync(int areaId, CancellationToken cancellationToken = default);
}

public interface IRouteRepository : IRepository<Route>
{
    Task<Route?> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default);
}

public interface ICouponRepository : IRepository<Coupon>
{
    Task<IReadOnlyList<Coupon>> GetByAirlineAsync(string airline, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Coupon>> GetExpiredCouponsAsync(CancellationToken cancellationToken = default);
}

public interface ITaxSlabRepository : IRepository<TaxSlab>
{
    Task<IReadOnlyList<TaxSlab>> GetActiveSlabsAsync(CancellationToken cancellationToken = default);
}

public interface IGlCodeCombinationRepository : IRepository<GlCodeCombination>
{
    Task<GlCodeCombination?> GetByCodeCombinationIdAsync(long codeCombinationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GlCodeCombination>> GetEnabledAsync(CancellationToken cancellationToken = default);
}

public interface IGuestRoomAvailabilityRepository : IRepository<GuestRoomAvailability>
{
    Task<IReadOnlyList<GuestRoomAvailability>> GetAvailableRoomsAsync(CancellationToken cancellationToken = default);
}
