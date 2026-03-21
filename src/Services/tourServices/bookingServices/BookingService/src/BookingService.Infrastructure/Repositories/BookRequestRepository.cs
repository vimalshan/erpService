using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence;

namespace BookingService.Infrastructure.Repositories;

public class BookRequestRepository(BookingDbContext context) : IBookRequestRepository
{
    public async Task<BookRequestMain?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return await context.BookRequestMains
            .Include(b => b.Tickets)
            .Include(b => b.Stays)
            .Include(b => b.Cabs)
            .Include(b => b.CostCentres)
            .Include(b => b.Others)
            .Include(b => b.Confirmations)
            .FirstOrDefaultAsync(b => b.BookMainId == id, ct);
    }

    public async Task<IReadOnlyList<BookRequestMain>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.BookRequestMains
            .Include(b => b.Tickets)
            .Include(b => b.Stays)
            .Include(b => b.Cabs)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<BookRequestMain>> GetByEmployeeAsync(string employeeSysId, CancellationToken ct = default)
    {
        return await context.BookRequestMains
            .Include(b => b.Tickets)
            .Include(b => b.Stays)
            .Include(b => b.Cabs)
            .Where(b => b.EmployeeSysId == employeeSysId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(BookRequestMain entity, CancellationToken ct = default)
    {
        await context.BookRequestMains.AddAsync(entity, ct);
    }

    public void Update(BookRequestMain entity)
    {
        context.BookRequestMains.Update(entity);
    }

    public void Delete(BookRequestMain entity)
    {
        context.BookRequestMains.Remove(entity);
    }
}
