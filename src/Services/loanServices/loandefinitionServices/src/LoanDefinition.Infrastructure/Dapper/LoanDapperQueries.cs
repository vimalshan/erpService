using Dapper;
using LoanDefinition.Application.DTOs;
using Microsoft.Data.SqlClient;

namespace LoanDefinition.Infrastructure.Dapper;

public interface ILoanDapperQueries
{
    Task<IEnumerable<LoanMasterDto>> GetAllLoansAsync();
    Task<LoanMasterDto?> GetLoanByIdAsync(long loanId);
    Task<IEnumerable<LoanInterestRateDto>> GetInterestRatesByLoanIdAsync(long loanId);
    Task<IEnumerable<LoanLimitRangeDto>> GetLimitRangesByLoanIdAsync(long loanId);
}

public class LoanDapperQueries(string connectionString) : ILoanDapperQueries
{
    public async Task<IEnumerable<LoanMasterDto>> GetAllLoansAsync()
    {
        const string sql = """
            SELECT m.LOAN_ID AS LoanId, m.LOAN_NAME AS LoanName, m.LOAN_PURPOSE AS LoanPurpose,
                   m.LOAN_APPLYToUNIT AS ApplyToUnit, m.LOAN_ORGID AS OrgId, m.LOAN_UNITID AS UnitId,
                   m.LOAN_TYPEID AS LoanTypeId, t.LOAN_NAME AS LoanTypeName,
                   m.LOAN_APPLYToCONFIRMEMP AS ApplyToConfirmedEmp,
                   m.LOAN_GRADECATAGORY AS GradeCategory, m.LOAN_APPLYToALLGRADE AS ApplyToAllGrade,
                   m.LOAN_GRADEID AS GradeId, m.LOAN_MINIMUMLIMIT AS MinimumLimit,
                   m.LOAN_MAXIMUMLIMIT AS MaximumLimit, m.LOAN_AUTOPAYONCOMPLETION AS AutoPayOnCompletion,
                   m.LOAN_ALLOWFORCECLOSE AS AllowForceClose, m.LOAN_ALLOWMULTIPLENOS AS AllowMultipleNos,
                   m.LOAN_ONCONFIRMATION AS OnConfirmation, m.LOAN_CHECKENTITLEMENT AS CheckEntitlement,
                   m.LOAN_RECOVERABLE AS Recoverable, m.LOAN_APPLICATIONNOS AS ApplicationNos,
                   m.LOAN_RECTYPE AS RecoveryType, m.LOAN_COMFACTOR AS CompoundingFactor,
                   m.LOAN_INTFREQUENCY AS InterestFrequency,
                   m.LOAN_EFFDATE AS EffectiveDate, m.LOAN_CLSDATE AS ClosureDate,
                   m.LOAN_BULKUPLOADALLOWED AS BulkUploadAllowed,
                   m.LOAN_POLICYFILENAME AS PolicyFileName,
                   m.LOAN_CREATEDBY AS CreatedBy, m.LOAN_CREATEDON AS CreatedOn,
                   m.LOAN_LASTMODIFIEDBY AS LastModifiedBy, m.LOAN_LASTMODIFIEDON AS LastModifiedOn
            FROM LOAN_MASTER m
            LEFT JOIN LOAN_TYPEMASTER t ON m.LOAN_TYPEID = t.LOAN_TYPE
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<LoanMasterDto>(sql);
    }

    public async Task<LoanMasterDto?> GetLoanByIdAsync(long loanId)
    {
        const string sql = """
            SELECT m.LOAN_ID AS LoanId, m.LOAN_NAME AS LoanName, m.LOAN_PURPOSE AS LoanPurpose,
                   m.LOAN_APPLYToUNIT AS ApplyToUnit, m.LOAN_ORGID AS OrgId, m.LOAN_UNITID AS UnitId,
                   m.LOAN_TYPEID AS LoanTypeId, t.LOAN_NAME AS LoanTypeName,
                   m.LOAN_APPLYToCONFIRMEMP AS ApplyToConfirmedEmp,
                   m.LOAN_GRADECATAGORY AS GradeCategory, m.LOAN_APPLYToALLGRADE AS ApplyToAllGrade,
                   m.LOAN_GRADEID AS GradeId, m.LOAN_MINIMUMLIMIT AS MinimumLimit,
                   m.LOAN_MAXIMUMLIMIT AS MaximumLimit, m.LOAN_AUTOPAYONCOMPLETION AS AutoPayOnCompletion,
                   m.LOAN_ALLOWFORCECLOSE AS AllowForceClose, m.LOAN_ALLOWMULTIPLENOS AS AllowMultipleNos,
                   m.LOAN_ONCONFIRMATION AS OnConfirmation, m.LOAN_CHECKENTITLEMENT AS CheckEntitlement,
                   m.LOAN_RECOVERABLE AS Recoverable, m.LOAN_APPLICATIONNOS AS ApplicationNos,
                   m.LOAN_RECTYPE AS RecoveryType, m.LOAN_COMFACTOR AS CompoundingFactor,
                   m.LOAN_INTFREQUENCY AS InterestFrequency,
                   m.LOAN_EFFDATE AS EffectiveDate, m.LOAN_CLSDATE AS ClosureDate,
                   m.LOAN_BULKUPLOADALLOWED AS BulkUploadAllowed,
                   m.LOAN_POLICYFILENAME AS PolicyFileName,
                   m.LOAN_CREATEDBY AS CreatedBy, m.LOAN_CREATEDON AS CreatedOn,
                   m.LOAN_LASTMODIFIEDBY AS LastModifiedBy, m.LOAN_LASTMODIFIEDON AS LastModifiedOn
            FROM LOAN_MASTER m
            LEFT JOIN LOAN_TYPEMASTER t ON m.LOAN_TYPEID = t.LOAN_TYPE
            WHERE m.LOAN_ID = @LoanId
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryFirstOrDefaultAsync<LoanMasterDto>(sql, new { LoanId = loanId });
    }

    public async Task<IEnumerable<LoanInterestRateDto>> GetInterestRatesByLoanIdAsync(long loanId)
    {
        const string sql = """
            SELECT LOANINT_RATEID AS RateId, LOANINT_LOANID AS LoanId,
                   LOANINT_EFFDATE AS EffectiveDate, LOANINT_CLSDATE AS ClosureDate,
                   LOANINT_RATE AS Rate, LOANINT_EMIAMT AS EmiAmount,
                   LOANINT_INSNOS AS InstallmentNos, LOANINT_RANGESPECIFIC AS RangeSpecific
            FROM LOAN_INTRATEMAST
            WHERE LOANINT_LOANID = @LoanId
            ORDER BY LOANINT_EFFDATE DESC
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<LoanInterestRateDto>(sql, new { LoanId = loanId });
    }

    public async Task<IEnumerable<LoanLimitRangeDto>> GetLimitRangesByLoanIdAsync(long loanId)
    {
        const string sql = """
            SELECT LOANLIMITRANGE_RATEID AS RangeRateId, LOANLIMITRANGE_LOANID AS LoanId,
                   LOANLIMITRANGE_MINYEAR AS MinYear, LOANLIMITRANGE_MAXYEAR AS MaxYear,
                   LOANLIMITRANGE_LOANAMOUNT AS LoanAmount,
                   LOANLIMITRANGE_EFFDATE AS EffectiveDate, LOANLIMITRANGE_CLSDATE AS ClosureDate,
                   LOANLIMITRANGE_INTRATE AS InterestRate
            FROM LOANLIMITRANGE_MAST
            WHERE LOANLIMITRANGE_LOANID = @LoanId
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<LoanLimitRangeDto>(sql, new { LoanId = loanId });
    }
}
