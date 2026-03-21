using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper)
    : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await repo.GetByIdAsync(request.CategoryId, ct)
            ?? throw new KeyNotFoundException($"Category {request.CategoryId} not found.");

        category.Update(request.Dto.CategoryName, request.Dto.Description, request.Dto.ParentCategoryId);
        await repo.UpdateAsync(category, ct);
        return mapper.Map<CategoryDto>(category);
    }
}
