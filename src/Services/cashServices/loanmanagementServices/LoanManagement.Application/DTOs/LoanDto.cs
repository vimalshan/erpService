namespace LoanManagement.Application.DTOs;

public record LoanDto(
    decimal LoanId,
    string LoanKey,
    decimal LoanOrgId,
    decimal? LoanOrgCurr,
    decimal? LoanCurr,
    DateTime LoanDate,
    decimal LoanTypeId,
    decimal LoanBankId,
    decimal LoanAmount,
    string? LoanStatus,
    DateTime LoanCreatedOn,
    List<DisbursementScheduleDto> Disbursements,
    List<InterestDto> Interests,
    List<RepaymentScheduleDto> Repayments
);
