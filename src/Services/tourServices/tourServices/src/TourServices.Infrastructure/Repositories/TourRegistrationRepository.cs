using Microsoft.EntityFrameworkCore;
using TourServices.Domain.Entities;
using TourServices.Domain.Interfaces;
using TourServices.Infrastructure.Persistence;

namespace TourServices.Infrastructure.Repositories;

public sealed class TourRegistrationRepository : ITourRegistrationRepository
{
    private readonly ApplicationDbContext _context;

    public TourRegistrationRepository(ApplicationDbContext context) => _context = context;

    public async Task<TourRegistration?> GetByIdAsync(long registrationId, CancellationToken ct = default)
        => await _context.TourRegistrations.FindAsync(new object[] { registrationId }, ct);

    public async Task<IEnumerable<TourRegistration>> GetByTourIdAsync(long tourId, CancellationToken ct = default)
        => await _context.TourRegistrations
            .Where(r => r.TourId == tourId)
            .OrderByDescending(r => r.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<TourRegistration>> GetByParticipantIdAsync(long participantId, CancellationToken ct = default)
        => await _context.TourRegistrations
            .Where(r => r.ParticipantId == participantId)
            .OrderByDescending(r => r.CreatedOn)
            .ToListAsync(ct);

    public async Task<int> GetActiveCountByTourAsync(long tourId, CancellationToken ct = default)
        => await _context.TourRegistrations
            .CountAsync(r => r.TourId == tourId && EF.Property<string>(r, "REGISTRATION_STATUS") == "A", ct);
}
