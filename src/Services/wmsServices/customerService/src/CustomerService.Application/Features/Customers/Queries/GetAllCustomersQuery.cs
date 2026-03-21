using CustomerService.Application.DTOs;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries;

public sealed record GetAllCustomersQuery : IRequest<IReadOnlyList<CustomerDto>>;
