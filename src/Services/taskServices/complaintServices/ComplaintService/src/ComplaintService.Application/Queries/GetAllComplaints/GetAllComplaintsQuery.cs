using ComplaintService.Application.DTOs;
using MediatR;

namespace ComplaintService.Application.Queries.GetAllComplaints;

public record GetAllComplaintsQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<ComplaintTicketDto>>;
