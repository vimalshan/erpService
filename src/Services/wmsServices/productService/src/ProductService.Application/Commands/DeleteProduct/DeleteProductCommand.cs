using MediatR;

namespace ProductService.Application.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int ProductId) : IRequest<bool>;
