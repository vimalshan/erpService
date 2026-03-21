using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConfigService.Infrastructure.Persistence.Repositories;

public class TravelCityRepository(ConfigDbContext context) : EfRepository<TravelCity, string>(context), ITravelCityRepository
{
    public async Task<TravelCity?> GetWithMapsAsync(string id, CancellationToken ct = default) =>
        await DbSet.Include(e => e.ModeMaps).Include(e => e.SectorMaps).FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<TravelCity>> GetByCountryAsync(string countryId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(c => c.CountryId == countryId).ToListAsync(ct);
}

public class TravelCountryRepository(ConfigDbContext context) : EfRepository<TravelCountry, string>(context), ITravelCountryRepository
{
    public async Task<TravelCountry?> GetWithMapsAsync(string id, CancellationToken ct = default) =>
        await DbSet.Include(e => e.ModeMaps).Include(e => e.SectorMaps).Include(e => e.CurrencyMaps)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
}

public class TravelClassRepository(ConfigDbContext context) : EfRepository<TravelClass, string>(context), ITravelClassRepository
{
    public async Task<IReadOnlyList<TravelClass>> GetByModeAsync(string modeId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(c => c.ModeId == modeId).ToListAsync(ct);
}

public class TravelContactRepository(ConfigDbContext context) : EfRepository<TravelContact, string>(context), ITravelContactRepository { }

public class TravelBusCitySectorMapRepository(ConfigDbContext context) : EfRepository<TravelBusCitySectorMap, string>(context), ITravelBusCitySectorMapRepository { }

public class TravelBuExcludeRepository(ConfigDbContext context) : EfRepository<TravelBuExclude, string>(context), ITravelBuExcludeRepository { }
