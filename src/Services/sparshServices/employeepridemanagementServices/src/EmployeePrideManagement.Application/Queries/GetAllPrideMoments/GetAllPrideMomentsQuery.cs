using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetAllPrideMoments;

public record GetAllPrideMomentsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResultDto<PrideMomentDto>>;
