using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed record DeactivateCustomerCommand(int CustomerId) : IRequest<bool>;
