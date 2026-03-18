using Dapper;
using GSTComplianceService.Application.Common.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GSTComplianceService.Infrastructure.Dapper;

public interface IGstDapperRepository
{
    Task<GstMainDto?> GetGstDetailsDapperAsync(long gstId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GstMainDto>> SearchGstByPanAsync(string panNo, CancellationToken cancellationToken = default);
}

public class GstDapperRepository : IGstDapperRepository
{
    private readonly string _connectionString;

    public GstDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<GstMainDto?> GetGstDetailsDapperAsync(long gstId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                g.GST_ID          AS GstId,
                g.GST_TYPE        AS GstType,
                g.GST_PANNO       AS GstPanNo,
                g.GST_EMAILID     AS GstEmailId,
                g.GST_MOBILENO    AS GstMobileNo,
                g.GST_CREATEDON   AS GstCreatedOn,
                g.GST_MODIFIEDON  AS GstModifiedOn,
                g.GST_VENDORNAME  AS GstVendorName,
                g.GST_VENDADDLINE1 AS GstVendAddLine1,
                g.GST_VENDCITY    AS GstVendCity,
                g.GST_VENDSTATE   AS GstVendState,
                g.GST_VENDPINCODE AS GstVendPincode,
                g.GST_REGISTRATIONTYPE AS GstRegistrationType,
                g.GST_CONTACTNAME AS GstContactName,
                g.GST_CONTACTEMAILID AS GstContactEmailId,
                g.GST_CONTACTMOBILENO AS GstContactMobileNo,
                g.GST_REMARKS     AS GstRemarks,
                g.GST_STATUS      AS GstStatus,
                g.GST_DIGITALFLAG AS GstDigitalFlag,
                g.GST_GSTNCOPY    AS GstGstnCopy
            FROM dbo.GST_MAIN g
            WHERE g.GST_ID = @GstId
            """;

        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryFirstOrDefaultAsync<GstMainDto>(new CommandDefinition(sql, new { GstId = gstId }, cancellationToken: cancellationToken));
        return result;
    }

    public async Task<IEnumerable<GstMainDto>> SearchGstByPanAsync(string panNo, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                GST_ID AS GstId, GST_TYPE AS GstType, GST_PANNO AS GstPanNo,
                GST_EMAILID AS GstEmailId, GST_MOBILENO AS GstMobileNo,
                GST_CREATEDON AS GstCreatedOn, GST_STATUS AS GstStatus,
                GST_DIGITALFLAG AS GstDigitalFlag, GST_VENDORNAME AS GstVendorName
            FROM dbo.GST_MAIN
            WHERE GST_PANNO LIKE @PanNo
            ORDER BY GST_CREATEDON DESC
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<GstMainDto>(new CommandDefinition(sql, new { PanNo = $"%{panNo}%" }, cancellationToken: cancellationToken));
    }
}
