using InventoryManagement.Application.DTOs;
using MediatR;

namespace InventoryManagement.Application.Queries.Items;

public record GetAllItemsQuery : IRequest<IEnumerable<ItemDto>>;

public record GetItemByIdQuery(int SciItemId) : IRequest<ItemDto?>;

public record GetItemsByProductQuery(int ProductId) : IRequest<IEnumerable<ItemDto>>;

public record GetItemByOracleCodeQuery(string OracleCode) : IRequest<ItemDto?>;
