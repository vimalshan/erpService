using DeductionService.Application.DTOs;
using DeductionService.Application.Interfaces;
using DeductionService.Domain.Entities;
using DeductionService.Domain.Interfaces;
using DeductionService.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace DeductionService.Application.CQRS.Commands.ProcessMonthlyDeduction;

public record ProcessMonthlyDeductionCommand(
    string MonthYear,
    long ProcessedByUserId) : IRequest<ProcessMonthlyDeductionResultDto>;

public class ProcessMonthlyDeductionCommandValidator : AbstractValidator<ProcessMonthlyDeductionCommand>
{
    public ProcessMonthlyDeductionCommandValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty().WithMessage("MonthYear is required.")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format.");
        RuleFor(x => x.ProcessedByUserId).GreaterThan(0);
    }
}

public class ProcessMonthlyDeductionCommandHandler(
    IAdhocPayDeductionRepository repository,
    IUnitOfWork unitOfWork,
    IMessagePublisher publisher)
    : IRequestHandler<ProcessMonthlyDeductionCommand, ProcessMonthlyDeductionResultDto>
{
    public async Task<ProcessMonthlyDeductionResultDto> Handle(
        ProcessMonthlyDeductionCommand request, CancellationToken ct)
    {
        var period = MonthYear.Parse(request.MonthYear);
        var deductions = (await repository.GetByMonthYearAsync(period, ct)).ToList();

        var totalAmount = deductions.Where(d => d.CancelFlag != "Y").Sum(d => d.PayAmount ?? 0);

        await publisher.PublishAsync("deduction.monthly.processed", new
        {
            MonthYear = request.MonthYear,
            ProcessedCount = deductions.Count,
            TotalAmount = totalAmount,
            ProcessedBy = request.ProcessedByUserId,
            ProcessedAt = DateTime.UtcNow
        }, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return new ProcessMonthlyDeductionResultDto(
            request.MonthYear,
            deductions.Count,
            totalAmount,
            true,
            null);
    }
}
