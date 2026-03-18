using Microsoft.EntityFrameworkCore;
using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Domain.Entities;
using MedicalVisit.Infrastructure.Persistence;

namespace MedicalVisit.Infrastructure.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly MedicalVisitDbContext _context;

    public VisitRepository(MedicalVisitDbContext context)
    {
        _context = context;
    }

    public async Task<VisitMainAggregate?> GetByIdAsync(string companyCode, long visitNumber, CancellationToken cancellationToken = default)
    {
        return await _context.VisitMains
            .Include(v => v.SubRecords)
            .FirstOrDefaultAsync(v => v.CompanyCode == companyCode && v.VisitNumber == visitNumber, cancellationToken);
    }

    public async Task<IEnumerable<VisitMainAggregate>> GetByDateRangeAsync(string companyCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.VisitMains
            .Include(v => v.SubRecords)
            .Where(v => v.CompanyCode == companyCode && v.VisitDate >= startDate && v.VisitDate <= endDate)
            .OrderByDescending(v => v.VisitDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VisitMainAggregate>> GetByMedicalUserIdAsync(string companyCode, string medicalUserId, CancellationToken cancellationToken = default)
    {
        return await _context.VisitMains
            .Include(v => v.SubRecords)
            .Where(v => v.CompanyCode == companyCode && v.MedicalUserId == medicalUserId)
            .OrderByDescending(v => v.VisitDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> GetNextVisitNumberAsync(string companyCode, CancellationToken cancellationToken = default)
    {
        var maxVisitNumber = await _context.VisitMains
            .Where(v => v.CompanyCode == companyCode)
            .MaxAsync(v => (long?)v.VisitNumber, cancellationToken);

        return (maxVisitNumber ?? 0) + 1;
    }

    public async Task<VisitMainAggregate> AddAsync(VisitMainAggregate visit, CancellationToken cancellationToken = default)
    {
        await _context.VisitMains.AddAsync(visit, cancellationToken);
        return visit;
    }

    public Task UpdateAsync(VisitMainAggregate visit, CancellationToken cancellationToken = default)
    {
        _context.VisitMains.Update(visit);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<VisitMainAggregate>> GetAllAsync(string companyCode, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await _context.VisitMains
            .Include(v => v.SubRecords)
            .Where(v => v.CompanyCode == companyCode)
            .OrderByDescending(v => v.VisitDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
