using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Queries.Items;
using InventoryManagement.Application.Queries.Products;
using MediatR;

namespace InventoryManagement.API.GraphQL;

public sealed class InventoryQuery
{
    public async Task<IEnumerable<ProductDto>> GetProducts(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllProductsQuery(), ct);

    public async Task<ProductDto?> GetProductById(
        int id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetProductByIdQuery(id), ct);

    public async Task<IEnumerable<ItemDto>> GetItems(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllItemsQuery(), ct);

    public async Task<ItemDto?> GetItemById(
        int id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetItemByIdQuery(id), ct);

    public async Task<ItemDto?> GetItemByOracleCode(
        string code, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetItemByOracleCodeQuery(code), ct);

    public async Task<IEnumerable<ItemDto>> GetItemsByProduct(
        int productId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetItemsByProductQuery(productId), ct);
}
