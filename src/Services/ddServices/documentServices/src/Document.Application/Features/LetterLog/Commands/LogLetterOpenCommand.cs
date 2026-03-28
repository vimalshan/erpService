using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        // DDLETTER_LOGHISTORY has no primary key (HasNoKey) — use raw SQL insert
        var sqlParams = new object?[]
        {
            log.LogSysId,
            log.IpAddress,
            log.OpenedOn,
            (object?)log.EmployeeSysId ?? (object?)null,
            (object?)log.LetterType ?? (object?)null,
            (object?)log.FinancialYearId ?? (object?)null
        };
        await _ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO DDLETTER_LOGHISTORY 
              (DDLETTER_LOGSYSID, DDLETTER_IPADDRESS, DDLETTER_OPENEDON, DDLETTER_EMPSYSID, DDLETTER_TYPE, DDLETTER_FINYEARID)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
            sqlParams);

        return new LetterLogHistoryDto(log.LogSysId, log.IpAddress, log.OpenedOn, log.EmployeeSysId, log.LetterType);
    }
}
