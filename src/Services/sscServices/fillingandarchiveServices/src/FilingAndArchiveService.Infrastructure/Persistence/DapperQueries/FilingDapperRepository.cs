using Dapper;
using FilingAndArchiveService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FilingAndArchiveService.Infrastructure.Persistence.DapperQueries;

public class FilingDapperRepository
{
    private readonly string _connectionString;

    public FilingDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("DefaultConnection connection string is not configured.");

    public async Task<IEnumerable<FileMasterDto>> SearchFilesAsync(
        string? orgId = null,
        string? fileNo = null,
        long? year = null,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                FILE_ID         AS FileId,
                FILE_ORGID      AS FileOrgId,
                FILE_YEAR       AS FileYear,
                FILE_NO         AS FileNo,
                FILE_STATUS     AS FileStatus,
                FILE_REMARKS    AS FileRemarks,
                FILE_PODNO      AS FilePodNo,
                FILE_COURIERNAME AS FileCourierName,
                FILE_CREATEDON  AS FileCreatedOn,
                FILE_CREATEDBY  AS FileCreatedBy,
                FILE_UPDATEDON  AS FileUpdatedOn,
                FILE_UPDATEDBY  AS FileUpdatedBy,
                FILE_DISPATCHEDON  AS FileDispatchedOn,
                FILE_DISPATCHEDBY  AS FileDispatchedBy
            FROM FILE_MASTER
            WHERE (@OrgId IS NULL OR FILE_ORGID = @OrgId)
              AND (@FileNo IS NULL OR FILE_NO = @FileNo)
              AND (@Year IS NULL OR FILE_YEAR = @Year)
              AND (@Status IS NULL OR FILE_STATUS = @Status)
            ORDER BY FILE_ID DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql,
            new { OrgId = orgId, FileNo = fileNo, Year = year, Status = status },
            cancellationToken: cancellationToken);

        return await connection.QueryAsync<FileMasterDto>(cmd);
    }

    public async Task<IEnumerable<FilingDocErrorDto>> GetErrorsByDocKeyAsync(
        string docKey,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                DOC_KEY         AS DocKey,
                REMARKS         AS Remarks,
                SYS_ID          AS SysId,
                ACCOUNTING_DATE AS AccountingDate,
                FLAG            AS Flag,
                STATUS          AS Status,
                SNO             AS Sno
            FROM FILINGDOC_ERROR_LIST
            WHERE DOC_KEY = @DocKey
            ORDER BY SNO
            """;

        await using var connection = new SqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { DocKey = docKey }, cancellationToken: cancellationToken);
        return await connection.QueryAsync<FilingDocErrorDto>(cmd);
    }
}
