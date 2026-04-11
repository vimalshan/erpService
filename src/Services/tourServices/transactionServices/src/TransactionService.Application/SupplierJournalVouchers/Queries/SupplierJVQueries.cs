using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.SupplierJournalVouchers.Queries;

public sealed record GetSupplierJVByIdQuery(long JvId) : IRequest<SupplierJVDto>;

public sealed class GetSupplierJVByIdQueryHandler(
    ISupplierJVRepository repository) : IRequestHandler<GetSupplierJVByIdQuery, SupplierJVDto>
{
    public async Task<SupplierJVDto> Handle(GetSupplierJVByIdQuery request, CancellationToken cancellationToken)
    {
        var jv = await repository.GetByIdAsync(request.JvId, cancellationToken)
            ?? throw new JournalVoucherNotFoundException(request.JvId);

        return new SupplierJVDto
        {
            JvId = jv.JvId,
            JvType = jv.JvType,
            JvDate = jv.JvDate,
            JvVendorId = jv.JvVendorId,
            JvOraRefNo = jv.JvOraRefNo,
            JvStatus = jv.JvStatus,
            JvRefInvNo = jv.JvRefInvNo,
            JvNetAmt = jv.JvNetAmt,
            JvTrnType = jv.JvTrnType,
            JvAdminId = jv.JvAdminId,
            Lines = jv.Lines.Select(l => new SupplierJVLineDto
            {
                JvSubId = l.JvSubId,
                JvId = l.JvId,
                JvBu = l.JvBu,
                JvAcCode = l.JvAcCode,
                JvDcFlag = l.JvDcFlag,
                JvTrnAmt = l.JvTrnAmt,
                JvRemarks = l.JvRemarks,
                JvSubType = l.JvSubType
            })
        };
    }
}

public sealed record GetAllSupplierJVsQuery(
    int Page = 1, int PageSize = 20, long? VendorId = null) : IRequest<IEnumerable<SupplierJVDto>>;

public sealed class GetAllSupplierJVsQueryHandler(
    ISupplierJVRepository repository) : IRequestHandler<GetAllSupplierJVsQuery, IEnumerable<SupplierJVDto>>
{
    public async Task<IEnumerable<SupplierJVDto>> Handle(GetAllSupplierJVsQuery request, CancellationToken cancellationToken)
    {
        var jvs = request.VendorId.HasValue
            ? await repository.GetByVendorIdAsync(request.VendorId.Value, cancellationToken)
            : await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        return jvs.Select(jv => new SupplierJVDto
        {
            JvId = jv.JvId,
            JvType = jv.JvType,
            JvDate = jv.JvDate,
            JvVendorId = jv.JvVendorId,
            JvOraRefNo = jv.JvOraRefNo,
            JvStatus = jv.JvStatus,
            JvRefInvNo = jv.JvRefInvNo,
            JvNetAmt = jv.JvNetAmt,
            JvTrnType = jv.JvTrnType,
            JvAdminId = jv.JvAdminId
        });
    }
}
