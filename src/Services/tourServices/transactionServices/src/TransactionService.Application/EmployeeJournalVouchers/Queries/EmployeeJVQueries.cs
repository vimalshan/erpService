using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.EmployeeJournalVouchers.Queries;

public sealed record GetEmployeeJVByIdQuery(long JvBatchId) : IRequest<EmployeeJVDto>;

public sealed class GetEmployeeJVByIdQueryHandler(
    IEmployeeJVRepository repository) : IRequestHandler<GetEmployeeJVByIdQuery, EmployeeJVDto>
{
    public async Task<EmployeeJVDto> Handle(
        GetEmployeeJVByIdQuery request,
        CancellationToken cancellationToken)
    {
        var jv = await repository.GetByIdAsync(request.JvBatchId, cancellationToken)
            ?? throw new JournalVoucherNotFoundException(request.JvBatchId);

        return new EmployeeJVDto
        {
            JvBatchId = jv.JvBatchId,
            JvTpId = jv.JvTpId,
            JvType = jv.JvType,
            JvDate = jv.JvDate,
            JvEmpSysId = jv.JvEmpSysId,
            JvStatus = jv.JvStatus,
            JvTrnType = jv.JvTrnType,
            JvOraRefNo = jv.JvOraRefNo,
            JvNetAmt = jv.JvNetAmt,
            JvPayUnitId = jv.JvPayUnitId,
            JvTrnRefNo = jv.JvTrnRefNo,
            Lines = jv.Lines.Select(l => new EmployeeJVLineDto
            {
                JvSubId = l.JvSubId,
                JvBatchId = l.JvBatchId,
                JvBu = l.JvBu,
                JvAcCode = l.JvAcCode,
                JvSubAcc = l.JvSubAcc,
                JvCcCode = l.JvCcCode,
                JvProduct = l.JvProduct,
                JvDcFlag = l.JvDcFlag,
                JvTrnAmt = l.JvTrnAmt,
                JvRemarks = l.JvRemarks,
                JvSubType = l.JvSubType
            })
        };
    }
}

public sealed record GetAllEmployeeJVsQuery(
    int Page = 1,
    int PageSize = 20,
    long? EmployeeId = null,
    string? Status = null) : IRequest<IEnumerable<EmployeeJVDto>>;

public sealed class GetAllEmployeeJVsQueryHandler(
    IEmployeeJVRepository repository) : IRequestHandler<GetAllEmployeeJVsQuery, IEnumerable<EmployeeJVDto>>
{
    public async Task<IEnumerable<EmployeeJVDto>> Handle(
        GetAllEmployeeJVsQuery request,
        CancellationToken cancellationToken)
    {
        var jvs = request.EmployeeId.HasValue
            ? await repository.GetByEmployeeIdAsync(request.EmployeeId.Value, cancellationToken)
            : await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        return jvs.Select(jv => new EmployeeJVDto
        {
            JvBatchId = jv.JvBatchId,
            JvTpId = jv.JvTpId,
            JvType = jv.JvType,
            JvDate = jv.JvDate,
            JvEmpSysId = jv.JvEmpSysId,
            JvStatus = jv.JvStatus,
            JvTrnType = jv.JvTrnType,
            JvOraRefNo = jv.JvOraRefNo,
            JvNetAmt = jv.JvNetAmt,
            JvPayUnitId = jv.JvPayUnitId,
            JvTrnRefNo = jv.JvTrnRefNo
        });
    }
}
