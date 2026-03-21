using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
