using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using EximManagement.Infrastructure.Services;
using HotChocolate.Authorization;

namespace EximManagement.API.GraphQL;

public class EximQuery
{
    [Authorize]
    public async Task<IEnumerable<EximProductDto>> GetProducts(
        [Service] IEximProductRepository productRepo, CancellationToken ct)
    {
        var products = await productRepo.GetAllAsync(ct);
        return products.Select(p => new EximProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode,
            LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn,
            Status = p.Status.ToString()
        });
    }

    [Authorize]
    public async Task<EximProductDto?> GetProductById(
        long productId, [Service] IEximProductRepository productRepo, CancellationToken ct)
    {
        var p = await productRepo.GetByIdAsync(productId, ct);
        if (p is null) return null;
        return new EximProductDto
        {
            ProductId = p.ProductId, ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode, LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn, Status = p.Status.ToString()
        };
    }

    [Authorize]
    public async Task<IEnumerable<EximDataFileDto>> GetDataFiles(
        [GraphQLType(typeof(string))] string? fileType,
        [Service] IEximDataFileRepository fileRepo, CancellationToken ct)
    {
        var files = fileType is null
            ? await fileRepo.GetAllAsync(ct)
            : await fileRepo.GetByTypeAsync(fileType, ct);

        return files.Select(f => new EximDataFileDto
        {
            FileId = f.FileId, FileType = f.FileType, FileName = f.FileName,
            OriginalCount = f.OriginalCount, FinalCount = f.FinalCount,
            FileUploadedBy = f.FileUploadedBy, FileUploadedOn = f.FileUploadedOn,
            Remarks = f.Remarks, FileSource = f.FileSource
        });
    }

    [Authorize]
    public async Task<IEnumerable<EximDataExportDto>> GetExportData(
        DateTime from, DateTime to, [Service] EximDapperService dapperService)
        => await dapperService.GetEximExportDataAsync(from, to);

    [Authorize]
    public async Task<IEnumerable<EximDataImportDto>> GetImportData(
        DateTime from, DateTime to, [Service] EximDapperService dapperService)
        => await dapperService.GetEximImportDataAsync(from, to);
}

public class EximMutation
{
    [Authorize]
    public async Task<EximProductDto> CreateProduct(
        string productName, string? oracleCode, long updatedBy,
        [Service] IEximProductRepository productRepo,
        [Service] Application.Interfaces.IUnitOfWork uow,
        CancellationToken ct)
    {
        var product = Domain.Entities.EximProduct.Create(productName, oracleCode, updatedBy);
        await productRepo.AddAsync(product, ct);
        await uow.SaveChangesAsync(ct);
        return new EximProductDto
        {
            ProductId = product.ProductId, ProductName = product.ProductName,
            ProductOracleCode = product.ProductOracleCode, LastUpdatedBy = product.LastUpdatedBy,
            LastUpdatedOn = product.LastUpdatedOn, Status = product.Status.ToString()
        };
    }
}
