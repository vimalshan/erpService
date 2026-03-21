using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed record ActivateCustomerCommand(int CustomerId) : IRequest<bool>;
