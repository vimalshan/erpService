using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed record DeleteCustomerCommand(int CustomerId) : IRequest<bool>;
