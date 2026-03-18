using AutoMapper;
using DeductionService.Application.DTOs;
using DeductionService.Application.Interfaces;
using DeductionService.Domain.Entities;
using DeductionService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DeductionService.Application.CQRS.Commands.CreateAdhocDeduction;

public record CreateAdhocDeductionCommand(
    long SystemId,
    long? CanteenUnit,
    decimal? PayAmount,
    string? EarningDeductionCode,
    long? EmployeeNumber,
    long EnteredByUserId,
    string? CompanyCode,
    string? GradeType) : IRequest<AdhocPayDeductionDto>;

public class CreateAdhocDeductionCommandValidator : AbstractValidator<CreateAdhocDeductionCommand>
{
    public CreateAdhocDeductionCommandValidator()
    {
        RuleFor(x => x.SystemId).GreaterThan(0).WithMessage("SystemId must be positive.");
        RuleFor(x => x.PayAmount).GreaterThan(0).WithMessage("PayAmount must be greater than zero.");
        RuleFor(x => x.EmployeeNumber).NotNull().GreaterThan(0).WithMessage("EmployeeNumber is required.");
        RuleFor(x => x.EnteredByUserId).GreaterThan(0).WithMessage("EnteredByUserId is required.");
        RuleFor(x => x.EarningDeductionCode).MaximumLength(6).When(x => x.EarningDeductionCode != null);
        RuleFor(x => x.CompanyCode).MaximumLength(3).When(x => x.CompanyCode != null);
    }
}

public class CreateAdhocDeductionCommandHandler(
    IAdhocPayDeductionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMessagePublisher publisher)
    : IRequestHandler<CreateAdhocDeductionCommand, AdhocPayDeductionDto>
{
    public async Task<AdhocPayDeductionDto> Handle(CreateAdhocDeductionCommand request, CancellationToken ct)
    {
        var deduction = AdhocPayDeduction.Create(
            request.SystemId,
            request.CanteenUnit,
            request.PayAmount,
            request.EarningDeductionCode,
            request.EmployeeNumber,
            request.EnteredByUserId);

        await repository.AddAsync(deduction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await publisher.PublishAsync("deduction.created", new
        {
            deduction.SystemId,
            deduction.EmployeeNumber,
            deduction.PayAmount,
            CreatedAt = DateTime.UtcNow
        }, ct);

        return mapper.Map<AdhocPayDeductionDto>(deduction);
    }
}
