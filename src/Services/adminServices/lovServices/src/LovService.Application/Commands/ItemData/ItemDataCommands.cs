using MediatR;

namespace LovService.Application.Commands.ItemData;

public record CreateItemDataCommand(string? CatName, string? ItemName, string? Make, string? Uom, int? Price) : IRequest<int>;

public record UpdateItemDataCommand(int Id, string? CatName, string? ItemName, string? Make, string? Uom, int? Price) : IRequest<bool>;

public record DeleteItemDataCommand(int Id) : IRequest<bool>;
