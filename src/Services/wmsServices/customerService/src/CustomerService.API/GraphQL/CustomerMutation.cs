using CustomerService.Application.DTOs;
using CustomerService.Application.Features.Customers.Commands;
using MediatR;

namespace CustomerService.API.GraphQL;

public class CustomerMutation
{
    public async Task<CustomerDto> CreateCustomer([Service] IMediator mediator, CreateCustomerCommand input, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<CustomerDto> UpdateCustomer([Service] IMediator mediator, UpdateCustomerCommand input, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> DeleteCustomer([Service] IMediator mediator, int customerId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new DeleteCustomerCommand(customerId), cancellationToken);
    }

    public async Task<bool> ActivateCustomer([Service] IMediator mediator, int customerId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new ActivateCustomerCommand(customerId), cancellationToken);
    }

    public async Task<bool> DeactivateCustomer([Service] IMediator mediator, int customerId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new DeactivateCustomerCommand(customerId), cancellationToken);
    }
}
