using MediatR;

namespace ProductService.Application.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(int CategoryId) : IRequest<bool>;
