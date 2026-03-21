using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IProductRepository repo, IMapper mapper)
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(request.ProductId, ct)
            ?? throw new KeyNotFoundException($"Product {request.ProductId} not found.");

        var dto = request.Dto;
        product.Update(dto.Name, dto.Description, dto.CategoryId,
            dto.UnitOfMeasure, dto.WeightPerUnit, dto.VolumePerUnit,
            dto.Price, dto.ReorderPoint, dto.ReorderQuantity);

        await repo.UpdateAsync(product, ct);
        return mapper.Map<ProductDto>(product);
    }
}
