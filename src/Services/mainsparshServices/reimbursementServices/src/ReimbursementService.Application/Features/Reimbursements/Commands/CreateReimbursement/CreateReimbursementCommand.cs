using AutoMapper;
using FluentValidation;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;
using ReimbursementService.Domain.ValueObjects;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.CreateReimbursement;

// ─── Command ───────────────────────────────────────────────────────────────────

public sealed record CreateReimbursementCommand(
    long EmpSysId,
    string ReimType,
    decimal Amount,
    string Currency,
    DateOnly ReimDate,
    DateOnly ExpenseDate,
    string? Description,
    string? Location,
    long CreatedBy
) : IRequest<ReimbursementDto>;

// ─── Validator ─────────────────────────────────────────────────────────────────

public sealed class CreateReimbursementCommandValidator : AbstractValidator<CreateReimbursementCommand>
{
    public CreateReimbursementCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0).WithMessage("Employee ID is required.");
        RuleFor(x => x.ReimType)
            .NotEmpty()
            .Must(t => Enum.TryParse<ReimbursementType>(t, true, out _))
            .WithMessage("Invalid reimbursement type.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ReimDate).NotEmpty();
        RuleFor(x => x.ExpenseDate).NotEmpty().LessThanOrEqualTo(x => x.ReimDate);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

// ─── Handler ───────────────────────────────────────────────────────────────────

public sealed class CreateReimbursementCommandHandler(
    IReimbursementRepository repository,
    IMapper mapper) : IRequestHandler<CreateReimbursementCommand, ReimbursementDto>
{
    public async Task<ReimbursementDto> Handle(CreateReimbursementCommand request, CancellationToken cancellationToken)
    {
        var type = Enum.Parse<ReimbursementType>(request.ReimType, true);
        var money = new Money(request.Amount, request.Currency);

        // Generate unique reference number
        var refNo = $"REIM-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        while (await repository.RefNoExistsAsync(refNo, cancellationToken))
            refNo = $"REIM-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        var entity = ReimbursementTransaction.Create(
            refNo,
            request.EmpSysId,
            type,
            money,
            request.ReimDate,
            request.ExpenseDate,
            request.Description,
            request.Location,
            request.CreatedBy);

        await repository.AddAsync(entity, cancellationToken);
        return mapper.Map<ReimbursementDto>(entity);
    }
}

