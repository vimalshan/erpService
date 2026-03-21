using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.CreateCategory;

public sealed record CreateCategoryCommand(CreateCategoryDto Dto) : IRequest<CategoryDto>;
