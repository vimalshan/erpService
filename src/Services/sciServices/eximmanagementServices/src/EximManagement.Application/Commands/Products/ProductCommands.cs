using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using EximManagement.Domain.Entities;
using MediatR;

namespace EximManagement.Application.Commands.Products;

// ─── Commands ─────────────────────────────────────────────────────────────────

public record CreateProductCommand(long ProductId, string ProductName, string? OracleCode, long UpdatedBy)
    : IRequest<EximProductDto>;

public record UpdateProductCommand(long ProductId, string ProductName, string? OracleCode, long UpdatedBy)
    : IRequest<EximProductDto>;

public record DeactivateProductCommand(long ProductId, long UpdatedBy) : IRequest<bool>;

// ─── Handlers ─────────────────────────────────────────────────────────────────

public class CreateProductCommandHandler(
    IEximProductRepository repo,
    IUnitOfWork uow) : IRequestHandler<CreateProductCommand, EximProductDto>
{
    public async Task<EximProductDto> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = EximProduct.Create(cmd.ProductId, cmd.ProductName, cmd.OracleCode, cmd.UpdatedBy);
        await repo.AddAsync(product, ct);
        await uow.SaveChangesAsync(ct);
        return MapToDto(product);
    }

    private static EximProductDto MapToDto(EximProduct p) => new()
    {
        ProductId = p.ProductId, ProductName = p.ProductName,
        ProductOracleCode = p.ProductOracleCode, LastUpdatedBy = p.LastUpdatedBy,
        LastUpdatedOn = p.LastUpdatedOn, Status = p.Status
    };
}

public class UpdateProductCommandHandler(
    IEximProductRepository repo,
    IUnitOfWork uow) : IRequestHandler<UpdateProductCommand, EximProductDto>
{
    public async Task<EximProductDto> Handle(UpdateProductCommand cmd, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(cmd.ProductId, ct)
            ?? throw new KeyNotFoundException($"Product {cmd.ProductId} not found.");
        product.Update(cmd.ProductName, cmd.OracleCode, cmd.UpdatedBy);
        await repo.UpdateAsync(product, ct);
        await uow.SaveChangesAsync(ct);
        return new EximProductDto
        {
            ProductId = product.ProductId, ProductName = product.ProductName,
            ProductOracleCode = product.ProductOracleCode, LastUpdatedBy = product.LastUpdatedBy,
            LastUpdatedOn = product.LastUpdatedOn, Status = product.Status
        };
    }
}

public class DeactivateProductCommandHandler(
    IEximProductRepository repo,
    IUnitOfWork uow) : IRequestHandler<DeactivateProductCommand, bool>
{
    public async Task<bool> Handle(DeactivateProductCommand cmd, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(cmd.ProductId, ct)
            ?? throw new KeyNotFoundException($"Product {cmd.ProductId} not found.");
        product.Deactivate(cmd.UpdatedBy);
        await repo.UpdateAsync(product, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
