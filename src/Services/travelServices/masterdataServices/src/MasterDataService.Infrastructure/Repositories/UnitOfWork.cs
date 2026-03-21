using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Data;

namespace MasterDataService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MasterDataDbContext _context;

    public UnitOfWork(MasterDataDbContext context)
    {
        _context = context;
        GuestHouses = new GuestHouseRepository(context);
        GuestHouseRooms = new GuestHouseRoomRepository(context);
        Areas = new AreaRepository(context);
        Routes = new RouteRepository(context);
        Coupons = new CouponRepository(context);
        TaxSlabs = new TaxSlabRepository(context);
        GlCodeCombinations = new GlCodeCombinationRepository(context);
        GuestRoomAvailabilities = new GuestRoomAvailabilityRepository(context);
    }

    public IGuestHouseRepository GuestHouses { get; }
    public IGuestHouseRoomRepository GuestHouseRooms { get; }
    public IAreaRepository Areas { get; }
    public IRouteRepository Routes { get; }
    public ICouponRepository Coupons { get; }
    public ITaxSlabRepository TaxSlabs { get; }
    public IGlCodeCombinationRepository GlCodeCombinations { get; }
    public IGuestRoomAvailabilityRepository GuestRoomAvailabilities { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
