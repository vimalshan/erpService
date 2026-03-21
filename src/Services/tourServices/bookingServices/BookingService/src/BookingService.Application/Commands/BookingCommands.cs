using MediatR;
using BookingService.Application.DTOs;

namespace BookingService.Application.Commands;

public record CreateBookingCommand(
    string TpStatus,
    string TpId,
    string EmployeeSysId,
    string Through,
    string AdminId,
    string Remarks,
    string Type,
    string? ProofType,
    string? FoodPreference,
    string? BudgetedCost,
    string? EmployeeCalendarId,
    List<CreateBookRequestTicketInput>? Tickets,
    List<CreateBookRequestStayInput>? Stays,
    List<CreateBookRequestCabInput>? Cabs,
    List<CreateBookRequestCostCentreInput>? CostCentres,
    List<CreateBookRequestOtherInput>? Others
) : IRequest<BookRequestMainDto>;

public record UpdateBookingCommand(
    string BookMainId,
    string Remarks,
    string? FoodPreference,
    string? BudgetedCost
) : IRequest<BookRequestMainDto>;

public record DeleteBookingCommand(string BookMainId) : IRequest<bool>;

public record ApproveBookingCommand(string BookMainId, string ApprovedBy) : IRequest<bool>;

public record ConfirmBookingCommand(
    string BookId,
    string Mode,
    string RefId,
    DateTime StartDate,
    DateTime EndDate,
    string Cost,
    string ClassId,
    string VendorId,
    string AdminRemarks
) : IRequest<BookConfirmationDto>;

public record CancelBookingCommand(string BookMainId, string Reason) : IRequest<bool>;
