using AutoMapper;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public sealed class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductDto>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;

    public RegisterProductCommandHandler(
        IProductRepository repo, IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<ProductDto> Handle(RegisterProductCommand request, CancellationToken ct)
    {
        var entity = new MainProductMaster
        {
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            UnitId = request.UnitId,
            ProductTypeId = request.ProductTypeId,
            CompanyUnitId = request.CompanyUnitId,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("inventory.product.created", new
        {
            entity.ProductId,
            entity.ProductName
        }, ct);

        return _mapper.Map<ProductDto>(entity);
    }
}
