using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using MediatR;

namespace EximManagement.Application.Queries.Products;

// ─── Queries ──────────────────────────────────────────────────────────────────

public record GetProductByIdQuery(long ProductId) : IRequest<EximProductDto?>;
public record GetAllProductsQuery : IRequest<IEnumerable<EximProductDto>>;

public record GetExportDataByDateRangeQuery(DateTime From, DateTime To)
    : IRequest<IEnumerable<EximDataExportDto>>;

public record GetImportDataByDateRangeQuery(DateTime From, DateTime To)
    : IRequest<IEnumerable<EximDataImportDto>>;

// ─── Handlers ─────────────────────────────────────────────────────────────────

public class GetProductByIdQueryHandler(IEximProductRepository repo)
    : IRequestHandler<GetProductByIdQuery, EximProductDto?>
{
    public async Task<EximProductDto?> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var p = await repo.GetByIdAsync(query.ProductId, ct);
        if (p is null) return null;
        return new EximProductDto
        {
            ProductId = p.ProductId, ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode, LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn, Status = p.Status
        };
    }
}

public class GetAllProductsQueryHandler(IEximProductRepository repo)
    : IRequestHandler<GetAllProductsQuery, IEnumerable<EximProductDto>>
{
    public async Task<IEnumerable<EximProductDto>> Handle(GetAllProductsQuery query, CancellationToken ct)
    {
        var products = await repo.GetAllAsync(ct);
        return products.Select(p => new EximProductDto
        {
            ProductId = p.ProductId, ProductName = p.ProductName,
            ProductOracleCode = p.ProductOracleCode, LastUpdatedBy = p.LastUpdatedBy,
            LastUpdatedOn = p.LastUpdatedOn, Status = p.Status
        });
    }
}

public class GetExportDataByDateRangeQueryHandler(IEximDataExportRepository repo)
    : IRequestHandler<GetExportDataByDateRangeQuery, IEnumerable<EximDataExportDto>>
{
    public async Task<IEnumerable<EximDataExportDto>> Handle(GetExportDataByDateRangeQuery q, CancellationToken ct)
    {
        var records = await repo.GetByDateRangeAsync(q.From, q.To, ct);
        return records.Select(r => new EximDataExportDto
        {
            DataId = r.DataId, EximDate = r.EximDate, HsCode = r.HsCode,
            ProdDesc = r.ProdDesc, CountryDest = r.CountryDest, PortDest = r.PortDest,
            StdQty = r.StdQty, StdUnit = r.StdUnit, FobInr = r.FobInr, FobDol = r.FobDol,
            ExpName = r.ExpName, ImpName = r.ImpName, ImpCountry = r.ImpCountry,
            Iec = r.Iec, SbNo = r.SbNo, EMonth = r.EMonth, FileId = r.FileId
        });
    }
}

public class GetImportDataByDateRangeQueryHandler(IEximDataImportRepository repo)
    : IRequestHandler<GetImportDataByDateRangeQuery, IEnumerable<EximDataImportDto>>
{
    public async Task<IEnumerable<EximDataImportDto>> Handle(GetImportDataByDateRangeQuery q, CancellationToken ct)
    {
        var records = await repo.GetByDateRangeAsync(q.From, q.To, ct);
        return records.Select(r => new EximDataImportDto
        {
            DataId = r.DataId, EximDate = r.EximDate, HsCode = r.HsCode,
            ProdDesc = r.ProdDesc, CountryOrg = r.CountryOrg, PortDest = r.PortDest,
            StdQty = r.StdQty, StdUnit = r.StdUnit, FobInr = r.FobInr, FobDol = r.FobDol,
            ImpName = r.ImpName, ExpName = r.ExpName, Iec = r.Iec,
            BeNo = r.BeNo, EMonth = r.EMonth, FileId = r.FileId
        });
    }
}
