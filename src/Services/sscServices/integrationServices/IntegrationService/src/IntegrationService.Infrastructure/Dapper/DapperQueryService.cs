using System.Data;
using Dapper;
using IntegrationService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IntegrationService.Infrastructure.Dapper;

public interface IDapperQueryService
{
    Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default);
    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(long poSeqId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VendorDto>> GetVendorsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OrganizationUnitDto>> GetOrganizationUnitsAsync(CancellationToken cancellationToken = default);
}

public class DapperQueryService(IConfiguration configuration) : IDapperQueryService
{
    private IDbConnection CreateConnection()
        => new SqlConnection(configuration.GetConnectionString("IntegrationDb"));

    public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT PO_SEQID AS PoSeqId, PO_OUID AS OracleOrgId, PO_ID AS OraclePoId,
                   PO_NO AS PoNumber, PO_VENDORSITEID AS VendorSiteId,
                   PO_DUEDAYS AS DueDays, PO_DUE_DAY_MONTHOFF AS DueDayMonthOffset,
                   PO_MONTHFORWARD AS MonthForward
            FROM ORA_POMAST
            """;
        var result = await connection.QueryAsync<PurchaseOrderDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return result;
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(long poSeqId, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT PO_SEQID AS PoSeqId, PO_OUID AS OracleOrgId, PO_ID AS OraclePoId,
                   PO_NO AS PoNumber, PO_VENDORSITEID AS VendorSiteId,
                   PO_DUEDAYS AS DueDays, PO_DUE_DAY_MONTHOFF AS DueDayMonthOffset,
                   PO_MONTHFORWARD AS MonthForward
            FROM ORA_POMAST
            WHERE PO_SEQID = @PoSeqId
            """;
        return await connection.QueryFirstOrDefaultAsync<PurchaseOrderDto>(
            new CommandDefinition(sql, new { PoSeqId = poSeqId }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<VendorDto>> GetVendorsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT VENDOR_ID AS VendorId, VENDOR_NAME AS VendorName, VENDOR_CODE AS VendorCode
            FROM ORA_VENDORMAST
            """;
        return await connection.QueryAsync<VendorDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<OrganizationUnitDto>> GetOrganizationUnitsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT OU_ID AS OuId, OU_NAME AS OuName, OU_BUID AS BuId
            FROM ORA_OUMAST
            """;
        return await connection.QueryAsync<OrganizationUnitDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }
}
