using Microsoft.EntityFrameworkCore;
using TourServices.Domain.Aggregates;
using TourServices.Domain.Interfaces;
using TourServices.Domain.ValueObjects;
using TourServices.Infrastructure.Persistence;

namespace TourServices.Infrastructure.Repositories;

public sealed class TourPackageRepository : ITourPackageRepository
{
    private readonly ApplicationDbContext _context;

    public TourPackageRepository(ApplicationDbContext context) => _context = context;

    public async Task<TourPackage?> GetByIdAsync(long tourId, CancellationToken ct = default)
        => await _context.TourPackages
            .Include(t => t.Registrations)
            .FirstOrDefaultAsync(t => t.TourId == tourId, ct);

    public async Task<IEnumerable<TourPackage>> GetAllAsync(CancellationToken ct = default)
        => await _context.TourPackages
            .Include(t => t.Registrations)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<TourPackage>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        TourStatus tourStatus;
        try { tourStatus = TourStatus.From(status); }
        catch (ArgumentException) { return []; }

        return await _context.TourPackages
            .Include(t => t.Registrations)
            .Where(t => t.TourStatus == tourStatus)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(ct);
    }

    public async Task AddAsync(TourPackage tourPackage, CancellationToken ct = default)
        => await _context.TourPackages.AddAsync(tourPackage, ct);

    public void Update(TourPackage tourPackage)
        => _context.TourPackages.Update(tourPackage);

    public void Delete(TourPackage tourPackage)
        => _context.TourPackages.Remove(tourPackage);
}
