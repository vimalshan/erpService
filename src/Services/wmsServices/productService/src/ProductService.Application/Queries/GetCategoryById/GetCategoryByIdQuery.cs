using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int CategoryId) : IRequest<CategoryDto?>;
