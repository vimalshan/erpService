using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.EmployeeJournalVouchers.Commands.CreateEmployeeJV;

public sealed record CreateEmployeeJVCommand : IRequest<EmployeeJVDto>
{
    public long JvBatchId { get; init; }
    public long JvTpId { get; init; }
    public string JvType { get; init; } = default!;
    public DateTime JvDate { get; init; }
    public long JvEmpSysId { get; init; }
    public string JvTrnType { get; init; } = default!;
    public decimal JvNetAmt { get; init; }
    public long JvPayUnitId { get; init; }
    public long CreatedBy { get; init; }
    public List<CreateEmployeeJVLineItem> Lines { get; init; } = [];
}

public sealed record CreateEmployeeJVLineItem
{
    public long JvSubId { get; init; }
    public string JvBu { get; init; } = default!;
    public string JvAcCode { get; init; } = default!;
    public string JvSubAcc { get; init; } = default!;
    public string JvCcCode { get; init; } = default!;
    public string JvProduct { get; init; } = default!;
    public string JvDcFlag { get; init; } = default!;
    public string JvTrnAmt { get; init; } = default!;
    public string JvIutaBu { get; init; } = default!;
    public string JvLoc { get; init; } = default!;
    public string JvRemarks { get; init; } = default!;
    public string JvLineFlag { get; init; } = default!;
    public string JvSubType { get; init; } = default!;
    public string? JvCombinationId { get; init; }
    public string? JvCombinationCode { get; init; }
}

public sealed class CreateEmployeeJVCommandValidator : AbstractValidator<CreateEmployeeJVCommand>
{
    public CreateEmployeeJVCommandValidator()
    {
        RuleFor(x => x.JvBatchId).GreaterThan(0);
        RuleFor(x => x.JvTpId).GreaterThan(0);
        RuleFor(x => x.JvType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.JvDate).NotEmpty();
        RuleFor(x => x.JvEmpSysId).GreaterThan(0);
        RuleFor(x => x.JvTrnType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.JvNetAmt).GreaterThan(0);
        RuleFor(x => x.JvPayUnitId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.JvSubId).GreaterThan(0);
            line.RuleFor(l => l.JvBu).NotEmpty().MaximumLength(25);
            line.RuleFor(l => l.JvAcCode).NotEmpty().MaximumLength(25);
            line.RuleFor(l => l.JvDcFlag).NotEmpty();
            line.RuleFor(l => l.JvTrnAmt).NotEmpty();
        });
    }
}

public sealed class CreateEmployeeJVCommandHandler(
    IEmployeeJVRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeJVCommand, EmployeeJVDto>
{
    public async Task<EmployeeJVDto> Handle(
        CreateEmployeeJVCommand request,
        CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.JvBatchId, cancellationToken))
            throw new InvalidOperationException($"Employee JV with Batch ID '{request.JvBatchId}' already exists.");

        var jv = EmployeeJournalVoucher.Create(
            request.JvBatchId, request.JvTpId, request.JvType, request.JvDate,
            request.JvEmpSysId, request.JvTrnType, request.JvNetAmt,
            request.JvPayUnitId, request.CreatedBy);

        foreach (var line in request.Lines)
        {
            jv.AddLine(Domain.Entities.EmployeeJVLine.Create(
                line.JvSubId, request.JvBatchId, line.JvBu, line.JvAcCode,
                line.JvSubAcc, line.JvCcCode, line.JvProduct, line.JvDcFlag,
                line.JvTrnAmt, line.JvIutaBu, line.JvLoc, line.JvRemarks,
                line.JvLineFlag, line.JvSubType, line.JvCombinationId,
                line.JvCombinationCode));
        }

        await repository.AddAsync(jv, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(jv);
    }

    private static EmployeeJVDto MapToDto(EmployeeJournalVoucher jv) => new()
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
