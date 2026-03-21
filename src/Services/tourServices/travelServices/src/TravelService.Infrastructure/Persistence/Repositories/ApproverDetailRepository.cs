using Microsoft.EntityFrameworkCore;
using TravelService.Domain.Entities;
using TravelService.Domain.Repositories;
using TravelService.Infrastructure.Persistence;

namespace TravelService.Infrastructure.Persistence.Repositories;

public class ApproverDetailRepository : IApproverDetailRepository
{
    private readonly TravelDbContext _context;

    public ApproverDetailRepository(TravelDbContext context) => _context = context;

    public async Task<IEnumerable<ApproverDetail>> GetByTourPlanAsync(string tourPlanId, CancellationToken cancellationToken = default)
        => await _context.ApproverDetails
            .Where(a => a.TourPlanId == tourPlanId)
            .ToListAsync(cancellationToken);

    public async Task<ApproverDetail> AddAsync(ApproverDetail detail, CancellationToken cancellationToken = default)
    {
        await _context.ApproverDetails.AddAsync(detail, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return detail;
    }
}
