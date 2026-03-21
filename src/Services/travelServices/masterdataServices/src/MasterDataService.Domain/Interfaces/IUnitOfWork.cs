namespace MasterDataService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGuestHouseRepository GuestHouses { get; }
    IGuestHouseRoomRepository GuestHouseRooms { get; }
    IAreaRepository Areas { get; }
    IRouteRepository Routes { get; }
    ICouponRepository Coupons { get; }
    ITaxSlabRepository TaxSlabs { get; }
    IGlCodeCombinationRepository GlCodeCombinations { get; }
    IGuestRoomAvailabilityRepository GuestRoomAvailabilities { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
