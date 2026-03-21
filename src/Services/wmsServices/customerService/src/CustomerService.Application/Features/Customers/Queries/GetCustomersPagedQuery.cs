using CustomerService.Application.DTOs;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries;

public sealed record GetCustomersPagedQuery(int Page = 1, int PageSize = 10, string? Search = null)
    : IRequest<PagedResultDto<CustomerDto>>;
