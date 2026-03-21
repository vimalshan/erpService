using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var category = new Category(dto.CategoryName, dto.Description, dto.ParentCategoryId);
        var created = await repo.AddAsync(category, ct);
        return mapper.Map<CategoryDto>(created);
    }
}
