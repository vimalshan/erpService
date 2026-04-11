using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.SupplierJournalVouchers.Commands.CreateSupplierJV;

public sealed record CreateSupplierJVCommand : IRequest<SupplierJVDto>
{
    public long JvId { get; init; }
    public string JvType { get; init; } = default!;
    public DateTime JvDate { get; init; }
    public long JvVendorId { get; init; }
    public long JvPayUnitId { get; init; }
    public string JvRefInvNo { get; init; } = default!;
    public decimal JvNetAmt { get; init; }
    public string JvTrnType { get; init; } = default!;
    public long JvOraVendorId { get; init; }
    public long JvAdminId { get; init; }
    public long JvInvBatchId { get; init; }
    public long JvOraSiteId { get; init; }
    public string JvCenvatApplicable { get; init; } = default!;
    public string JvDocKeyNo { get; init; } = default!;
    public long CreatedBy { get; init; }
    public List<CreateSupplierJVLineItem> Lines { get; init; } = [];
}

public sealed record CreateSupplierJVLineItem
{
    public long JvSubId { get; init; }
    public string JvBu { get; init; } = default!;
    public string JvAcCode { get; init; } = default!;
    public string JvSubAcc { get; init; } = default!;
    public string JvCcCode { get; init; } = default!;
    public string JvProduct { get; init; } = default!;
    public string JvDcFlag { get; init; } = default!;
    public decimal JvTrnAmt { get; init; }
    public string JvLoc { get; init; } = default!;
    public string JvRemarks { get; init; } = default!;
    public string JvLineFlag { get; init; } = default!;
    public string JvCombinationId { get; init; } = default!;
    public string JvSubType { get; init; } = default!;
    public string JvIutaBu { get; init; } = default!;
    public long JvTpId { get; init; }
    public long JvBatchSubId { get; init; }
    public string? JvCombinationCode { get; init; }
}

public sealed class CreateSupplierJVCommandValidator : AbstractValidator<CreateSupplierJVCommand>
{
    public CreateSupplierJVCommandValidator()
    {
        RuleFor(x => x.JvId).GreaterThan(0);
        RuleFor(x => x.JvType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.JvDate).NotEmpty();
        RuleFor(x => x.JvVendorId).GreaterThan(0);
        RuleFor(x => x.JvNetAmt).GreaterThan(0);
        RuleFor(x => x.JvRefInvNo).NotEmpty().MaximumLength(25);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class CreateSupplierJVCommandHandler(
    ISupplierJVRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSupplierJVCommand, SupplierJVDto>
{
    public async Task<SupplierJVDto> Handle(
        CreateSupplierJVCommand request,
        CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.JvId, cancellationToken))
            throw new InvalidOperationException($"Supplier JV with ID '{request.JvId}' already exists.");

        var jv = SupplierJournalVoucher.Create(
            request.JvId, request.JvType, request.JvDate, request.JvVendorId,
            request.JvPayUnitId, request.JvRefInvNo, request.JvNetAmt,
            request.JvTrnType, request.JvOraVendorId, request.JvAdminId,
            request.JvInvBatchId, request.JvOraSiteId,
            request.JvCenvatApplicable, request.JvDocKeyNo, request.CreatedBy);

        foreach (var line in request.Lines)
        {
            jv.AddLine(Domain.Entities.SupplierJVLine.Create(
                line.JvSubId, request.JvId, line.JvBu, line.JvAcCode,
                line.JvSubAcc, line.JvCcCode, line.JvProduct, line.JvDcFlag,
                line.JvTrnAmt, line.JvLoc, line.JvRemarks, line.JvLineFlag,
                line.JvCombinationId, line.JvSubType, line.JvIutaBu,
                line.JvTpId, line.JvBatchSubId, line.JvCombinationCode));
        }

        await repository.AddAsync(jv, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
