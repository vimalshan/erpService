using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using EximManagement.Infrastructure.Services;
using HotChocolate.Authorization;

namespace EximManagement.API.GraphQL;

public class EximQuery(
    IEximProductRepository productRepo,
    IEximDataFileRepository fileRepo,
    EximDapperService dapperService)
{
    [Authorize]
    public async Task<IEnumerable<EximProductDto>> GetProducts(CancellationToken ct)
    {
        var products = await productRepo.GetAllAsync(ct);
        return products.Select(p => new EximProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode,
            LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn,
            Status = p.Status
        });
    }

    [Authorize]
    public async Task<EximProductDto?> GetProductById(long productId, CancellationToken ct)
    {
        var p = await productRepo.GetByIdAsync(productId, ct);
        if (p is null) return null;
        return new EximProductDto
        {
            ProductId = p.ProductId, ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode, LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn, Status = p.Status
        };
    }

    [Authorize]
    public async Task<IEnumerable<EximDataFileDto>> GetDataFiles([GraphQLType(typeof(string))] string? fileType, CancellationToken ct)
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
    public async Task<IEnumerable<EximDataExportDto>> GetExportData(DateTime from, DateTime to)
        => await dapperService.GetEximExportDataAsync(from, to);

    [Authorize]
    public async Task<IEnumerable<EximDataImportDto>> GetImportData(DateTime from, DateTime to)
        => await dapperService.GetEximImportDataAsync(from, to);
}

public class EximMutation(
    IEximProductRepository productRepo,
    Application.Interfaces.IUnitOfWork uow)
{
    [Authorize]
    public async Task<EximProductDto> CreateProduct(
        long productId, string productName, string? oracleCode, long updatedBy, CancellationToken ct)
    {
        var product = Domain.Entities.EximProduct.Create(productId, productName, oracleCode, updatedBy);
        await productRepo.AddAsync(product, ct);
        await uow.SaveChangesAsync(ct);
        return new EximProductDto
        {
            ProductId = product.ProductId, ProductName = product.ProductName,
            ProductOracleCode = product.ProductOracleCode, LastUpdatedBy = product.LastUpdatedBy,
            LastUpdatedOn = product.LastUpdatedOn, Status = product.Status
        };
    }
}
