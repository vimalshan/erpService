using BookingService.Domain.Aggregates;
using BookingService.Domain.Entities;
using BookingService.Domain.Enums;
using BookingService.Domain.Interfaces;
using BookingService.Domain.ValueObjects;
using BookingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context) => _context = context;

    public async Task<BookingAggregate?> GetByIdAsync(long bookingNumber, CancellationToken ct = default)
    {
        var entity = await _context.BookingRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.BkBokNum == bookingNumber, ct);

        return entity is null ? null : MapToAggregate(entity);
    }

    public async Task AddAsync(BookingAggregate booking, CancellationToken ct = default)
    {
        var entity = MapToEntity(booking);
        await _context.BookingRequests.AddAsync(entity, ct);
        _context.AddDomainEvents(booking.DomainEvents);
        booking.ClearDomainEvents();
    }

    public async Task UpdateAsync(BookingAggregate booking, CancellationToken ct = default)
    {
        var entity = await _context.BookingRequests
            .FirstOrDefaultAsync(r => r.BkBokNum == booking.Id, ct);

        if (entity is null) return;

        entity.BkAppSts = booking.Status switch
        {
            BookingStatus.Confirmed => "C",
            BookingStatus.CancellationRequested => "K",
            _ => "N"
        };
        entity.BkCnfNum = booking.ConfirmationNumber;
        entity.BkCanDat = booking.CancelledOn;
        entity.BkCanRem = booking.CancellationRemarks;
        entity.BkCanUsr = booking.CancelledBy;

        _context.AddDomainEvents(booking.DomainEvents);
        booking.ClearDomainEvents();
    }

    public async Task<IEnumerable<BookingRequest>> GetByUserAsync(string userCode, CancellationToken ct = default)
        => await _context.BookingRequests
            .Where(r => r.BkUsrCod == userCode)
            .OrderByDescending(r => r.BkAppDat)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<long> GetNextBookingNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.BookingRequests
            .MaxAsync(r => (decimal?)r.BkBokNum, ct) ?? 0m;
        return (long)max + 1;
    }

    private static BookingAggregate MapToAggregate(BookingRequest entity)
    {
        var bookingType = entity.BkBokTyp switch
        {
            "S" => BookingType.Stay,
            "L" => BookingType.LocalConveyance,
            _ => BookingType.Travel
        };
        var dateRange = DateRange.Create(
            entity.BkFroDat ?? DateTime.UtcNow,
            entity.BkRetDat ?? DateTime.UtcNow.AddDays(1));

        var booking = BookingAggregate.Create(
            (long)entity.BkBokNum,
            entity.BkUsrCod ?? string.Empty,
            entity.BkUsrNum ?? 0,
            bookingType,
            dateRange,
            entity.BkFroCit ?? 0,
            entity.BkToCit ?? 0,
            entity.BkFroLoc ?? string.Empty,
            entity.BkToLoc ?? string.Empty,
            PersonName.Create(entity.BkPerNam ?? "Unknown"),
            entity.BkBudAmt.HasValue ? Money.Create(entity.BkBudAmt.Value) : null,
            entity.BkAirCod,
            entity.BkTraCls);

        booking.ClearDomainEvents(); // don't re-raise creation event on load
        return booking;
    }

    private static BookingRequest MapToEntity(BookingAggregate booking) => new()
    {
        BkBokNum = booking.Id,
        BkSrlNum = 1,
        BkBokTyp = booking.BookingType switch
        {
            BookingType.Stay => "S",
            BookingType.LocalConveyance => "L",
            _ => "T"
        },
        BkUsrCod = booking.UserCode,
        BkUsrNum = booking.UserNum,
        BkFroDat = booking.TravelDates.From,
        BkRetDat = booking.TravelDates.To,
        BkFroCit = booking.FromCity,
        BkToCit = booking.ToCity,
        BkFroLoc = booking.FromLocation,
        BkToLoc = booking.ToLocation,
        BkPerNam = booking.PersonName.FullName,
        BkBudAmt = booking.BudgetAmount.Amount,
        BkAppSts = "N",
        BkAdmSlf = "N",
        BkPerSts = "S",
        BkAppDat = DateTime.UtcNow,
        BkAirCod = booking.AirlineCode,
        BkTraCls = booking.TravelClass
    };
}

public class BookingConfirmationRepository : IBookingConfirmationRepository
{
    private readonly BookingDbContext _context;

    public BookingConfirmationRepository(BookingDbContext context) => _context = context;

    public async Task<BookingConfirmation?> GetByIdAsync(long confirmationNumber, CancellationToken ct = default)
        => await _context.BookingConfirmations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.BkCnfNum == confirmationNumber, ct);

    public async Task AddAsync(BookingConfirmation confirmation, CancellationToken ct = default)
        => await _context.BookingConfirmations.AddAsync(confirmation, ct);

    public async Task<long> GetNextConfirmationNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.BookingConfirmations
            .MaxAsync(c => (long?)c.BkCnfNum, ct) ?? 0L;
        return max + 1;
    }
}

public class CouponRepository : ICouponRepository
{
    private readonly BookingDbContext _context;

    public CouponRepository(BookingDbContext context) => _context = context;

    public async Task<CouponMain?> GetByIdAsync(long couponId, CancellationToken ct = default)
        => await _context.CouponMains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CpnCupId == couponId, ct);

    public async Task AddAsync(CouponMain coupon, CancellationToken ct = default)
        => await _context.CouponMains.AddAsync(coupon, ct);
}
