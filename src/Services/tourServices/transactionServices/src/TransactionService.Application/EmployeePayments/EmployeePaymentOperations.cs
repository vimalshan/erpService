using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.EmployeePayments;

public sealed record CreateEmployeePaymentCommand : IRequest<EmployeePaymentDto>
{
    public long PayId { get; init; }
    public long PayTpId { get; init; }
    public string PayTrnType { get; init; } = default!;
    public long PayEmpSysId { get; init; }
    public long PayUnitId { get; init; }
    public string PayMode { get; init; } = default!;
    public string PayType { get; init; } = default!;
    public decimal PayAmount { get; init; }
    public long PayRefId { get; init; }
    public long PayBatchId { get; init; }
    public long PayJvId { get; init; }
    public long CreatedBy { get; init; }
}

public sealed class CreateEmployeePaymentCommandValidator : AbstractValidator<CreateEmployeePaymentCommand>
{
    public CreateEmployeePaymentCommandValidator()
    {
        RuleFor(x => x.PayId).GreaterThan(0);
        RuleFor(x => x.PayTpId).GreaterThan(0);
        RuleFor(x => x.PayTrnType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PayEmpSysId).GreaterThan(0);
        RuleFor(x => x.PayAmount).GreaterThan(0);
        RuleFor(x => x.PayMode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PayType).NotEmpty().MaximumLength(3);
    }
}

public sealed class CreateEmployeePaymentCommandHandler(
    IEmployeePaymentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeePaymentCommand, EmployeePaymentDto>
{
    public async Task<EmployeePaymentDto> Handle(
        CreateEmployeePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = EmployeePayment.Create(
            request.PayId, request.PayTpId, request.PayTrnType, request.PayEmpSysId,
            request.PayUnitId, request.PayMode, request.PayType, request.PayAmount,
            request.PayRefId, request.PayBatchId, request.PayJvId, request.CreatedBy);

        await repository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new EmployeePaymentDto
        {
            PayId = payment.PayId,
            PayTpId = payment.PayTpId,
            PayTrnType = payment.PayTrnType,
            PayEmpSysId = payment.PayEmpSysId,
            PayUnitId = payment.PayUnitId,
            PayMode = payment.PayMode,
            PayType = payment.PayType,
            PayDate = payment.PayDate,
            PayAmount = payment.PayAmount,
            PayRefId = payment.PayRefId,
            PayBatchId = payment.PayBatchId,
            PayJvId = payment.PayJvId
        };
    }
}

// Queries

public sealed record GetEmployeePaymentsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<EmployeePaymentDto>>;

public sealed class GetEmployeePaymentsByEmployeeQueryHandler(
    IEmployeePaymentRepository repository) : IRequestHandler<GetEmployeePaymentsByEmployeeQuery, IEnumerable<EmployeePaymentDto>>
{
    public async Task<IEnumerable<EmployeePaymentDto>> Handle(
        GetEmployeePaymentsByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var payments = await repository.GetByEmployeeIdAsync(request.EmpSysId, cancellationToken);

        return payments.Select(p => new EmployeePaymentDto
        {
            PayId = p.PayId,
            PayTpId = p.PayTpId,
            PayTrnType = p.PayTrnType,
            PayEmpSysId = p.PayEmpSysId,
            PayUnitId = p.PayUnitId,
            PayMode = p.PayMode,
            PayType = p.PayType,
            PayDate = p.PayDate,
            PayAmount = p.PayAmount,
            PayRefId = p.PayRefId,
            PayBatchId = p.PayBatchId,
            PayJvId = p.PayJvId
        });
    }
}

public sealed record GetEmployeePaymentByIdQuery(long PayId) : IRequest<EmployeePaymentDto>;

public sealed class GetEmployeePaymentByIdQueryHandler(
    IEmployeePaymentRepository repository) : IRequestHandler<GetEmployeePaymentByIdQuery, EmployeePaymentDto>
{
    public async Task<EmployeePaymentDto> Handle(
        GetEmployeePaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var p = await repository.GetByIdAsync(request.PayId, cancellationToken)
            ?? throw new EmployeePaymentNotFoundException(request.PayId);

        return new EmployeePaymentDto
        {
            PayId = p.PayId,
            PayTpId = p.PayTpId,
            PayTrnType = p.PayTrnType,
            PayEmpSysId = p.PayEmpSysId,
            PayUnitId = p.PayUnitId,
            PayMode = p.PayMode,
            PayType = p.PayType,
            PayDate = p.PayDate,
            PayAmount = p.PayAmount,
            PayRefId = p.PayRefId,
            PayBatchId = p.PayBatchId,
            PayJvId = p.PayJvId
        };
    }
}
