using MediatR;

namespace ComplaintService.Application.Commands.CloseComplaint;

public record CloseComplaintCommand(
    decimal TicketNum,
    string? FinalRemarks,
    decimal ClosedBy
) : IRequest<Unit>;
