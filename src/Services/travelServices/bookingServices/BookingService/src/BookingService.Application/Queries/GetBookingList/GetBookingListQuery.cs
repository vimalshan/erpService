using MediatR;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using FluentValidation;

namespace BookingService.Application.Queries.GetBookingList;

// ── Query ─────────────────────────────────────────────────────────────────────
public record GetBookingListQuery(string UserCode, int Page = 1, int PageSize = 20) : IRequest<IEnumerable<BookingListDto>>;

// ── Validator ─────────────────────────────────────────────────────────────────
public class GetBookingListQueryValidator : AbstractValidator<GetBookingListQuery>
{
    public GetBookingListQueryValidator()
    {
        RuleFor(x => x.UserCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

// ── Handler ──────────────────────────────────────────────────────────────────
public class GetBookingListQueryHandler : IRequestHandler<GetBookingListQuery, IEnumerable<BookingListDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingListQueryHandler(IBookingRepository bookingRepository)
        => _bookingRepository = bookingRepository;

    public async Task<IEnumerable<BookingListDto>> Handle(GetBookingListQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByUserAsync(request.UserCode, cancellationToken);
        return bookings
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BookingListDto(
                (long)b.BkBokNum,
                b.BkUsrCod ?? string.Empty,
                b.BkBokTyp ?? "T",
                b.BkFroDat ?? DateTime.MinValue,
                b.BkRetDat ?? DateTime.MinValue,
                b.BkAppSts ?? "N",
                b.BkPerNam ?? string.Empty));
    }
}
