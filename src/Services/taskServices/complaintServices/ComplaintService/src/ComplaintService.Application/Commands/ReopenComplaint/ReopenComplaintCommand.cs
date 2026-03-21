using MediatR;

namespace ComplaintService.Application.Commands.ReopenComplaint;

public record ReopenComplaintCommand(
    decimal TicketNum,
    string Remarks,
    decimal ReopenedBy
) : IRequest<Unit>;
