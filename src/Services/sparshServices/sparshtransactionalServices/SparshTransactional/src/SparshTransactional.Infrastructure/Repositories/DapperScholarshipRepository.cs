using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SparshTransactional.Application.DTOs;

namespace SparshTransactional.Infrastructure.Repositories;

public interface IDapperScholarshipRepository
{
    Task<IEnumerable<ScholarshipApplicationDto>> GetApplicationsByStatusAsync(string status);
    Task<IEnumerable<ScholarshipDisbursementDto>> GetPendingDisbursementsAsync();
    Task<ScholarshipApplicationDto?> GetApplicationByIdAsync(long id);
}

public class DapperScholarshipRepository : IDapperScholarshipRepository
{
    private readonly string _connectionString;

    public DapperScholarshipRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ScholarshipApplicationDto>> GetApplicationsByStatusAsync(string status)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT APPLICATION_ID as ApplicationId, EMP_STUDENT_ID as StudentId,
                   SCHOLARSHIP_ID as ScholarshipId, APPLICATION_DATE as ApplicationDate,
                   FAMILY_INCOME as FamilyIncome, APPLICATION_STATUS as ApplicationStatus,
                   APPROVED_AMOUNT as ApprovedAmount, APPROVED_BY as ApprovedBy,
                   REJECTION_REASON as RejectionReason, CREATED_BY as CreatedBy,
                   CREATED_ON as CreatedOn, UPDATED_ON as UpdatedOn
            FROM SCHOLARSHIP_APPLICATION
            WHERE APPLICATION_STATUS = @Status
            ORDER BY APPLICATION_DATE DESC
            """;
        return await connection.QueryAsync<ScholarshipApplicationDto>(sql, new { Status = status });
    }

    public async Task<IEnumerable<ScholarshipDisbursementDto>> GetPendingDisbursementsAsync()
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT DISBURSEMENT_ID as DisbursementId, APPLICATION_ID as ApplicationId,
                   STUDENT_ID as StudentId, SCHOLARSHIP_ID as ScholarshipId,
                   DISBURSEMENT_AMOUNT as DisbursementAmount, DISBURSEMENT_DATE as DisbursementDate,
                   DISBURSEMENT_STATUS as DisbursementStatus, PAYMENT_REFERENCE as PaymentReference,
                   CREATED_BY as CreatedBy, CREATED_ON as CreatedOn, UPDATED_ON as UpdatedOn
            FROM SCHOLARSHIP_DISBURSEMENT
            WHERE DISBURSEMENT_STATUS = 'P'
            ORDER BY CREATED_ON ASC
            """;
        return await connection.QueryAsync<ScholarshipDisbursementDto>(sql);
    }

    public async Task<ScholarshipApplicationDto?> GetApplicationByIdAsync(long id)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT APPLICATION_ID as ApplicationId, EMP_STUDENT_ID as StudentId,
                   SCHOLARSHIP_ID as ScholarshipId, APPLICATION_DATE as ApplicationDate,
                   FAMILY_INCOME as FamilyIncome, APPLICATION_STATUS as ApplicationStatus,
                   APPROVED_AMOUNT as ApprovedAmount, APPROVED_BY as ApprovedBy,
                   REJECTION_REASON as RejectionReason, CREATED_BY as CreatedBy,
                   CREATED_ON as CreatedOn, UPDATED_ON as UpdatedOn
            FROM SCHOLARSHIP_APPLICATION
            WHERE APPLICATION_ID = @Id
            """;
        return await connection.QueryFirstOrDefaultAsync<ScholarshipApplicationDto>(sql, new { Id = id });
    }
}
