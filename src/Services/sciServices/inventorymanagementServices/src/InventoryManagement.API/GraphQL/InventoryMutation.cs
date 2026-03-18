using InventoryManagement.Application.Commands.Items;
using InventoryManagement.Application.Commands.Products;
using InventoryManagement.Application.DTOs;
using MediatR;

namespace InventoryManagement.API.GraphQL;

public sealed class InventoryMutation
{
    public async Task<ProductDto> RegisterProduct(
        RegisterProductCommand command, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<ItemDto> RegisterItem(
        RegisterItemCommand command, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(command, ct);
}
