using ComplaintService.Application.DTOs;
using MediatR;

namespace ComplaintService.Application.Queries.GetComplaintById;

public record GetComplaintByIdQuery(decimal TicketNum) : IRequest<ComplaintTicketDto?>;
