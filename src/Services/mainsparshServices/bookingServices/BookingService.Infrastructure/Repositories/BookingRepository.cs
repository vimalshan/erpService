using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories;

public class BookingRepository(BookingDbContext context) : IBookingRepository
{
    public async Task<BookMain?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.BookMains
            .Include(b => b.Records)
            .Include(b => b.Attendees)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<BookMain?> GetByAppNoAsync(string appNo, CancellationToken cancellationToken = default)
        => await context.BookMains
            .Include(b => b.Records)
            .Include(b => b.Attendees)
            .FirstOrDefaultAsync(b => b.BookingAppNo == appNo, cancellationToken);

    public async Task<IEnumerable<BookMain>> GetAllAsync(int page, int pageSize, string? statusFilter, CancellationToken cancellationToken = default)
    {
        var query = context.BookMains
            .Include(b => b.Records)
            .Include(b => b.Attendees)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(statusFilter))
            query = query.Where(b => b.Status == BookingService.Domain.ValueObjects.BookingStatus.From(statusFilter));

        return await query
            .OrderByDescending(b => b.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(string? statusFilter, CancellationToken cancellationToken = default)
    {
        var query = context.BookMains.AsQueryable();

        if (!string.IsNullOrWhiteSpace(statusFilter))
            query = query.Where(b => b.Status == BookingService.Domain.ValueObjects.BookingStatus.From(statusFilter));

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(BookMain booking, CancellationToken cancellationToken = default)
        => await context.BookMains.AddAsync(booking, cancellationToken);

    public void Update(BookMain booking)
        => context.BookMains.Update(booking);

    public async Task UpdateAsync(BookMain booking, CancellationToken cancellationToken = default)
    {
        context.BookMains.Update(booking);
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<BookMain>> GetBookingsByStatusAsync(BookingService.Domain.ValueObjects.BookingStatus status, CancellationToken cancellationToken = default)
        => await context.BookMains
            .Include(b => b.Records)
            .Include(b => b.Attendees)
            .Where(b => b.Status == status)
            .ToListAsync(cancellationToken);

    public void Remove(BookMain booking)
        => context.BookMains.Remove(booking);
}
