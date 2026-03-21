using ComplaintService.Application.DTOs;
using MediatR;

namespace ComplaintService.Application.Queries.GetComplaintsByGroup;

public record GetComplaintsByGroupQuery(decimal GroupId) : IRequest<IEnumerable<ComplaintTicketDto>>;
