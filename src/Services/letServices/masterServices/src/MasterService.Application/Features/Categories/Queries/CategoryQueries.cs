using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Categories.Queries;

public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
public record GetCategoryByCodeQuery(string CategoryCode) : IRequest<CategoryDto?>;
