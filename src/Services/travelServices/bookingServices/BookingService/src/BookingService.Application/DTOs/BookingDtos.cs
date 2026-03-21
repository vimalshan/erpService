namespace BookingService.Application.DTOs;

public record BookingRequestDto(
    long BookingNumber,
    string UserCode,
    long UserNum,
    string BookingType,
    DateTime DepartureDate,
    DateTime ReturnDate,
    long FromCity,
    long ToCity,
    string FromLocation,
    string ToLocation,
    string PersonName,
    decimal BudgetAmount,
    string Status,
    long? ConfirmationNumber,
    DateTime? CancelledOn,
    string? CancellationRemarks);

public record BookingConfirmationDto(
    long ConfirmationNumber,
    long BookingNumber,
    long ModeOfTravel,
    long? FromCity,
    long? ToCity,
    DateTime? DepartureDate,
    DateTime? ReturnDate,
    long? VendorCode,
    string? TicketNumber,
    string? AdminRemarks,
    string Status);

public record CreateBookingRequestDto(
    string UserCode,
    long UserNum,
    string BookingType,
    DateTime DepartureDate,
    DateTime ReturnDate,
    long FromCity,
    long ToCity,
    string FromLocation,
    string ToLocation,
    string PersonName,
    decimal? BudgetAmount,
    string? AirlineCode,
    long? TravelClass);

public record ConfirmBookingRequestDto(
    long BookingNumber,
    long ModeOfTravel,
    long? VendorCode,
    string? TicketNumber,
    string? AdminRemarks);

public record CancelBookingRequestDto(
    long BookingNumber,
    string CancellationRemarks,
    string CancelledBy);

public record BookingListDto(
    long BookingNumber,
    string UserCode,
    string BookingType,
    DateTime DepartureDate,
    DateTime ReturnDate,
    string Status,
    string PersonName);
