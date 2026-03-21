using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IProductRepository repo, IMapper mapper)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var product = new Product(
            dto.Sku, dto.Name, dto.Description, dto.CategoryId,
            dto.UnitOfMeasure, dto.WeightPerUnit, dto.VolumePerUnit,
            dto.Price, dto.ReorderPoint, dto.ReorderQuantity);

        var created = await repo.AddAsync(product, ct);
        return mapper.Map<ProductDto>(created);
    }
}
