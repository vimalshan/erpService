using CustomerService.Application.DTOs;
using CustomerService.Application.Features.Customers.Queries;
using MediatR;

namespace CustomerService.API.GraphQL;

public class CustomerQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<CustomerDto>> GetCustomers([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllCustomersQuery(), cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerById([Service] IMediator mediator, int customerId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetCustomerByIdQuery(customerId), cancellationToken);
    }

    public async Task<PagedResultDto<CustomerDto>> GetCustomersPaged(
        [Service] IMediator mediator,
        int page = 1, int pageSize = 10, string? search = null,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetCustomersPagedQuery(page, pageSize, search), cancellationToken);
    }
}
