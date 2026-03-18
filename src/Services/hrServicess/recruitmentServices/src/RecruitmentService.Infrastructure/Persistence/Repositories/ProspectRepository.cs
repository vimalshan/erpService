using Microsoft.EntityFrameworkCore;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Infrastructure.Persistence;

namespace RecruitmentService.Infrastructure.Persistence.Repositories;

public class ProspectRepository : IProspectRepository
{
    private readonly RecruitmentDbContext _context;

    public ProspectRepository(RecruitmentDbContext context) => _context = context;

    public async Task<Prospect?> GetByIdAsync(decimal userId, CancellationToken ct = default)
        => await _context.Prospects
            .Include(p => p.Addresses)
            .Include(p => p.Qualifications)
            .Include(p => p.References)
            .Include(p => p.Trainings)
            .FirstOrDefaultAsync(p => p.WebUserId == userId, ct);

    public async Task<Prospect?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Prospects
            .FirstOrDefaultAsync(p => p.EmailId == email, ct);

    public async Task<IEnumerable<Prospect>> GetAllAsync(CancellationToken ct = default)
        => await _context.Prospects.OrderByDescending(p => p.CreatedOn).ToListAsync(ct);

    public async Task AddAsync(Prospect prospect, CancellationToken ct = default)
        => await _context.Prospects.AddAsync(prospect, ct);

    public void Update(Prospect prospect)
        => _context.Prospects.Update(prospect);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await _context.Prospects.AnyAsync(p => p.EmailId == email, ct);
}
