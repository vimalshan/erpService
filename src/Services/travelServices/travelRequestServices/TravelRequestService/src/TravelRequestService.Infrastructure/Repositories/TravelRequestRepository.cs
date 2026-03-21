using Microsoft.EntityFrameworkCore;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Interfaces;
using TravelRequestService.Infrastructure.Data;

namespace TravelRequestService.Infrastructure.Repositories;

public class TravelRequestRepository : ITravelRequestRepository
{
    private readonly TravelDbContext _context;

    public TravelRequestRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<TravelMain?> GetByIdAsync(long planNumber, string companyCode, CancellationToken cancellationToken = default)
    {
        return await _context.TravelMains
            .Include(t => t.SubDetails)
            .Include(t => t.Agendas)
            .Include(t => t.Advances)
            .Include(t => t.ApprovalRemarks)
            .FirstOrDefaultAsync(t => t.PlanNumber == planNumber && t.CompanyCode == companyCode, cancellationToken);
    }

    public async Task<IReadOnlyList<TravelMain>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TravelMains
            .Include(t => t.Agendas)
            .OrderByDescending(t => t.AppliedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TravelMain>> GetByUserAsync(long userNumber, CancellationToken cancellationToken = default)
    {
        return await _context.TravelMains
            .Include(t => t.Agendas)
            .Where(t => t.UserNumber == userNumber)
            .OrderByDescending(t => t.AppliedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TravelMain travelMain, CancellationToken cancellationToken = default)
    {
        await _context.TravelMains.AddAsync(travelMain, cancellationToken);
    }

    public Task UpdateAsync(TravelMain travelMain, CancellationToken cancellationToken = default)
    {
        _context.TravelMains.Update(travelMain);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long planNumber, string companyCode, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TravelMains
            .FirstOrDefaultAsync(t => t.PlanNumber == planNumber && t.CompanyCode == companyCode, cancellationToken);

        if (entity is not null)
            _context.TravelMains.Remove(entity);
    }
}
