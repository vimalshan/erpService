using AutoMapper;
using FluentValidation;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;
using ReimbursementService.Domain.ValueObjects;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.UpdateReimbursement;

public sealed record UpdateReimbursementCommand(
    long ReimId,
    string ReimType,
    decimal Amount,
    string Currency,
    DateOnly ReimDate,
    DateOnly ExpenseDate,
    string? Description,
    string? Location,
    long UpdatedBy
) : IRequest<ReimbursementDto>;

public sealed class UpdateReimbursementCommandValidator : AbstractValidator<UpdateReimbursementCommand>
{
    public UpdateReimbursementCommandValidator()
    {
        RuleFor(x => x.ReimId).GreaterThan(0);
        RuleFor(x => x.ReimType).NotEmpty().Must(t => Enum.TryParse<ReimbursementType>(t, true, out _));
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ReimDate).NotEmpty();
        RuleFor(x => x.ExpenseDate).LessThanOrEqualTo(x => x.ReimDate);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public sealed class UpdateReimbursementCommandHandler(
    IReimbursementRepository repository,
    IMapper mapper) : IRequestHandler<UpdateReimbursementCommand, ReimbursementDto>
{
    public async Task<ReimbursementDto> Handle(UpdateReimbursementCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken)
            ?? throw new KeyNotFoundException($"Reimbursement {request.ReimId} not found.");

        var type = Enum.Parse<ReimbursementType>(request.ReimType, true);
        var money = new Money(request.Amount, request.Currency);

        entity.Update(type, money, request.ReimDate, request.ExpenseDate, request.Description, request.Location, request.UpdatedBy);
        await repository.UpdateAsync(entity, cancellationToken);
        return mapper.Map<ReimbursementDto>(entity);
    }
}
