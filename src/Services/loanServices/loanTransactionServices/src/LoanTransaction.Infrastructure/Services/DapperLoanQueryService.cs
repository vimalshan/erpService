using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoanTransaction.Infrastructure.Services;

public class DapperLoanQueryService
{
    private readonly string _connectionString;

    public DapperLoanQueryService(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("DefaultConnection is not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<decimal> GetEmiAmountAsync(
        decimal principal, decimal annualRate, int tenureMonths)
    {
        await using var conn = CreateConnection();
        var result = await conn.QuerySingleAsync<decimal>(
            "SELECT dbo.fn_GetEMIAmount(@P_LoanAmount, @P_InterestRate, @P_TenureMonths)",
            new { P_LoanAmount = principal, P_InterestRate = annualRate, P_TenureMonths = tenureMonths });
        return result;
    }

    public async Task<decimal> GetLoanEligibilityAsync(int employeeId)
    {
        await using var conn = CreateConnection();
        var result = await conn.QuerySingleAsync<decimal>(
            "SELECT dbo.fn_GetLoanEligibility(@P_EmpID)",
            new { P_EmpID = employeeId });
        return result;
    }

    public async Task<string> ExecuteApplyForLoanAsync(
        int empId, int loanTypeId, decimal amount, int tenure, decimal interestRate)
    {
        await using var conn = CreateConnection();
        var p = new DynamicParameters();
        p.Add("P_EmpID", empId);
        p.Add("P_LoanTypeID", loanTypeId);
        p.Add("P_LoanAmount", amount);
        p.Add("P_TenureMonths", tenure);
        p.Add("P_InterestRate", interestRate);
        p.Add("P_Message", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
        await conn.ExecuteAsync("dbo.usp_ApplyForLoan", p, commandType: System.Data.CommandType.StoredProcedure);
        return p.Get<string>("P_Message") ?? string.Empty;
    }

    public async Task<string> ExecuteApproveLoanApplicationAsync(
        string loanNo, int approvedBy, decimal approvedAmount)
    {
        await using var conn = CreateConnection();
        var p = new DynamicParameters();
        p.Add("P_LoanNo", loanNo);
        p.Add("P_ApprovedBy", approvedBy);
        p.Add("P_ApprovedAmount", approvedAmount);
        p.Add("P_Message", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
        await conn.ExecuteAsync("dbo.usp_ApproveLoanApplication", p, commandType: System.Data.CommandType.StoredProcedure);
        return p.Get<string>("P_Message") ?? string.Empty;
    }

    public async Task<string> ExecuteRecordEmiPaymentAsync(string loanNo, int installmentNo, decimal amountPaid)
    {
        await using var conn = CreateConnection();
        var p = new DynamicParameters();
        p.Add("P_LoanNo", loanNo);
        p.Add("P_InstallmentNo", installmentNo);
        p.Add("P_AmountPaid", amountPaid);
        p.Add("P_Message", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
        await conn.ExecuteAsync("dbo.usp_RecordEMIPayment", p, commandType: System.Data.CommandType.StoredProcedure);
        return p.Get<string>("P_Message") ?? string.Empty;
    }
}
