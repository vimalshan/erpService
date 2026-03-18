using FluentValidation;
using MediatR;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;
using Document.Domain.Entities;

namespace Document.Application.Features.LetterLog.Commands;

public record LogLetterOpenCommand(
    decimal LogSysId,
    string IpAddress,
    decimal? EmployeeSysId,
    string? LetterType,
    decimal? FinancialYearId) : IRequest<LetterLogHistoryDto>;

public class LogLetterOpenCommandValidator : AbstractValidator<LogLetterOpenCommand>
{
    public LogLetterOpenCommandValidator()
    {
        RuleFor(x => x.IpAddress).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LogSysId).GreaterThan(0);
    }
}

public class LogLetterOpenCommandHandler : IRequestHandler<LogLetterOpenCommand, LetterLogHistoryDto>
{
    private readonly IApplicationDbContext _ctx;

    public LogLetterOpenCommandHandler(IApplicationDbContext ctx) => _ctx = ctx;

    public async Task<LetterLogHistoryDto> Handle(LogLetterOpenCommand request, CancellationToken cancellationToken)
    {
        var log = LetterLogHistory.Create(
            request.LogSysId,
            request.IpAddress,
            request.EmployeeSysId,
            request.LetterType,
            request.FinancialYearId);

        await _ctx.LetterLogHistories.AddAsync(log, cancellationToken);
        await _ctx.SaveChangesAsync(cancellationToken);

        return new LetterLogHistoryDto(log.LogSysId, log.IpAddress, log.OpenedOn, log.EmployeeSysId, log.LetterType);
    }
}
