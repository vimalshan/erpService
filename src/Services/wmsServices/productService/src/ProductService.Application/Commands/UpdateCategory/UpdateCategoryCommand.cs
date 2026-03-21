using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(int CategoryId, UpdateCategoryDto Dto) : IRequest<CategoryDto>;
