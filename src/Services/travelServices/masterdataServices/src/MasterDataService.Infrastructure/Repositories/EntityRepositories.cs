using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Repositories;

public class GuestHouseRepository : Repository<GuestHouse>, IGuestHouseRepository
{
    public GuestHouseRepository(MasterDataDbContext context) : base(context) { }

    public async Task<GuestHouse?> GetByAdminCodeAsync(long adminCode, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(g => g.AdminCode == adminCode, cancellationToken);

    public async Task<IReadOnlyList<GuestHouse>> GetWithRoomsAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Include(g => g.Rooms).ToListAsync(cancellationToken);
}

public class GuestHouseRoomRepository : Repository<GuestHouseRoom>, IGuestHouseRoomRepository
{
    public GuestHouseRoomRepository(MasterDataDbContext context) : base(context) { }

    public async Task<IReadOnlyList<GuestHouseRoom>> GetByGuestHouseCodeAsync(long guestHouseCode, CancellationToken cancellationToken = default)
        => await _dbSet.Where(r => r.GuestHouseCode == guestHouseCode).ToListAsync(cancellationToken);
}

public class AreaRepository : Repository<Area>, IAreaRepository
{
    public AreaRepository(MasterDataDbContext context) : base(context) { }

    public async Task<Area?> GetByAreaIdAsync(int areaId, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(a => a.AreaId == areaId, cancellationToken);
}

public class RouteRepository : Repository<Domain.Entities.Route>, IRouteRepository
{
    public RouteRepository(MasterDataDbContext context) : base(context) { }

    public async Task<Domain.Entities.Route?> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(r => r.RouteId == routeId, cancellationToken);
}

public class CouponRepository : Repository<Coupon>, ICouponRepository
{
    public CouponRepository(MasterDataDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Coupon>> GetByAirlineAsync(string airline, CancellationToken cancellationToken = default)
        => await _dbSet.Where(c => c.Airline == airline).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Coupon>> GetExpiredCouponsAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Where(c => c.ValidTill != null && c.ValidTill < DateTime.UtcNow).ToListAsync(cancellationToken);
}

public class TaxSlabRepository : Repository<TaxSlab>, ITaxSlabRepository
{
    public TaxSlabRepository(MasterDataDbContext context) : base(context) { }

    public async Task<IReadOnlyList<TaxSlab>> GetActiveSlabsAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Where(t => t.CloseDate == null || t.CloseDate > DateTime.UtcNow).ToListAsync(cancellationToken);
}

public class GlCodeCombinationRepository : Repository<GlCodeCombination>, IGlCodeCombinationRepository
{
    public GlCodeCombinationRepository(MasterDataDbContext context) : base(context) { }

    public async Task<GlCodeCombination?> GetByCodeCombinationIdAsync(long codeCombinationId, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(g => g.CodeCombinationId == codeCombinationId, cancellationToken);

    public async Task<IReadOnlyList<GlCodeCombination>> GetEnabledAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Where(g => g.EnabledFlag).ToListAsync(cancellationToken);
}

public class GuestRoomAvailabilityRepository : Repository<GuestRoomAvailability>, IGuestRoomAvailabilityRepository
{
    public GuestRoomAvailabilityRepository(MasterDataDbContext context) : base(context) { }

    public async Task<IReadOnlyList<GuestRoomAvailability>> GetAvailableRoomsAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Where(r => r.RoomStatus == 'A').ToListAsync(cancellationToken);
}
