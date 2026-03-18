using AutoMapper;
using LoanService.Application.Common;
using LoanService.Application.DTOs;
using LoanService.Domain.Interfaces;
using MediatR;

namespace LoanService.Application.Loans.Queries;

public class GetLoanByIdHandler : IRequestHandler<GetLoanByIdQuery, Result<LoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoanByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<LoanDto>> Handle(GetLoanByIdQuery req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        return loan is null
            ? Result<LoanDto>.Failure("Loan not found.")
            : Result<LoanDto>.Success(_mapper.Map<LoanDto>(loan));
    }
}

public class GetLoansByMemberHandler : IRequestHandler<GetLoansByMemberQuery, Result<IReadOnlyList<LoanDto>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoansByMemberHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<LoanDto>>> Handle(GetLoansByMemberQuery req, CancellationToken ct)
    {
        var loans = await _uow.Loans.GetByMemberIdAsync(req.MemberId, ct);
        return Result<IReadOnlyList<LoanDto>>.Success(_mapper.Map<IReadOnlyList<LoanDto>>(loans));
    }
}

public class GetActiveLoansHandler : IRequestHandler<GetActiveLoansQuery, Result<IReadOnlyList<LoanDto>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetActiveLoansHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<LoanDto>>> Handle(GetActiveLoansQuery req, CancellationToken ct)
    {
        var loans = await _uow.Loans.GetActiveLoansAsync(ct);
        return Result<IReadOnlyList<LoanDto>>.Success(_mapper.Map<IReadOnlyList<LoanDto>>(loans));
    }
}

public class GetActiveLoansSummaryHandler : IRequestHandler<GetActiveLoansSummaryQuery, Result<IEnumerable<ActiveLoanDto>>>
{
    private readonly ILoanDapperRepository _dapper;

    public GetActiveLoansSummaryHandler(ILoanDapperRepository dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<IEnumerable<ActiveLoanDto>>> Handle(GetActiveLoansSummaryQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT 
                lm.LOAN_NO AS LoanNo,
                lm.LOAN_MEMBER_ID AS MemberId,
                lm.LOAN_AMOUNT AS LoanAmount,
                lm.LOAN_PRINCIPALOS AS PrincipalOutstanding,
                lm.LOAN_DATE AS LoanDate,
                lm.LOAN_APPROVAL_DATE AS ApprovalDate,
                COUNT(lr.REPAY_ID) AS RemainingInstallments
            FROM LOAN_MAIN lm
            LEFT JOIN LOAN_REPAYMENT lr ON lm.LOAN_NO = lr.LOAN_NO AND lr.REPAY_STATUS = 'O'
            WHERE lm.LOAN_STATUS = 'A'
            GROUP BY lm.LOAN_NO, lm.LOAN_MEMBER_ID, lm.LOAN_AMOUNT, lm.LOAN_PRINCIPALOS, lm.LOAN_DATE, lm.LOAN_APPROVAL_DATE
            """;

        var results = await _dapper.QueryAsync<ActiveLoanDto>(sql, ct: ct);
        return Result<IEnumerable<ActiveLoanDto>>.Success(results);
    }
}
