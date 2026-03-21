using MediatR;
using BookingService.Application.DTOs;
using BookingService.Domain.Aggregates;
using BookingService.Domain.Enums;
using BookingService.Domain.Interfaces;
using BookingService.Domain.ValueObjects;
using FluentValidation;

namespace BookingService.Application.Commands.CreateBooking;

// ── Command ──────────────────────────────────────────────────────────────────
public record CreateBookingCommand(CreateBookingRequestDto Request) : IRequest<long>;

// ── Validator ────────────────────────────────────────────────────────────────
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.Request.UserCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.Request.UserNum).GreaterThan(0);
        RuleFor(x => x.Request.BookingType).NotEmpty().Length(1)
            .Must(t => "STL".Contains(t)).WithMessage("BookingType must be S, T, or L.");
        RuleFor(x => x.Request.DepartureDate).GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Departure date cannot be in the past.");
        RuleFor(x => x.Request.ReturnDate).GreaterThanOrEqualTo(x => x.Request.DepartureDate)
            .WithMessage("Return date must be on or after departure date.");
        RuleFor(x => x.Request.FromCity).GreaterThan(0);
        RuleFor(x => x.Request.ToCity).GreaterThan(0);
        RuleFor(x => x.Request.PersonName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Request.BudgetAmount).GreaterThanOrEqualTo(0).When(x => x.Request.BudgetAmount.HasValue);
    }
}

// ── Handler ──────────────────────────────────────────────────────────────────
public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, long>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _uow;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork uow)
    {
        _bookingRepository = bookingRepository;
        _uow = uow;
    }

    public async Task<long> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Request;
        var bookingNumber = await _bookingRepository.GetNextBookingNumberAsync(cancellationToken);

        var booking = BookingAggregate.Create(
            bookingNumber,
            dto.UserCode,
            dto.UserNum,
            Enum.Parse<BookingType>(dto.BookingType switch
            {
                "S" => nameof(BookingType.Stay),
                "T" => nameof(BookingType.Travel),
                "L" => nameof(BookingType.LocalConveyance),
                _ => throw new ArgumentException("Invalid booking type")
            }),
            DateRange.Create(dto.DepartureDate, dto.ReturnDate),
            dto.FromCity,
            dto.ToCity,
            dto.FromLocation ?? string.Empty,
            dto.ToLocation ?? string.Empty,
            PersonName.Create(dto.PersonName),
            dto.BudgetAmount.HasValue ? Money.Create(dto.BudgetAmount.Value) : null,
            dto.AirlineCode,
            dto.TravelClass);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return bookingNumber;
    }
}
