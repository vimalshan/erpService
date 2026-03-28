using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;
using Document.Domain.Entities;

namespace Document.Application.Features.GeneratedLetters.Commands;

public record GenerateLetterCommand(
    decimal? CreatedByPin,
    decimal? EmployeePin,
    string? EmployeeName,
    string? LetterType,
    DateTime? EffectiveDate,
    string? FinalRating,
    string? SignatoryName,
    string? SignatoryDesignation,
    decimal? AppraisalBasicPay,
    decimal? AppraisalFlexiPay) : IRequest<GeneratedLetterDto>;

public class GenerateLetterCommandValidator : AbstractValidator<GenerateLetterCommand>
{
    public GenerateLetterCommandValidator()
    {
        RuleFor(x => x.LetterType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.EmployeeName).MaximumLength(150);
    }
}

public class GenerateLetterCommandHandler : IRequestHandler<GenerateLetterCommand, GeneratedLetterDto>
{
    private readonly IApplicationDbContext _ctx;
    private readonly IMessagePublisher _publisher;

    public GenerateLetterCommandHandler(IApplicationDbContext ctx, IMessagePublisher publisher)
        => (_ctx, _publisher) = (ctx, publisher);

    public async Task<GeneratedLetterDto> Handle(GenerateLetterCommand request, CancellationToken cancellationToken)
    {
        var letter = GeneratedLetter.Create(
            request.CreatedByPin,
            request.EmployeePin,
            request.EmployeeName,
            request.LetterType,
            request.EffectiveDate);

        // DD_GENERATELETTER has no primary key (HasNoKey) — use raw SQL insert
        var sqlParams = new object?[]
        {
            (object?)request.CreatedByPin ?? (object?)null,
            (object?)request.EmployeePin ?? (object?)null,
            (object?)request.EmployeeName ?? (object?)null,
            (object?)request.LetterType ?? (object?)null,
            (object?)request.EffectiveDate ?? (object?)null,
            (object?)letter.PrintDate ?? (object?)null
        };
        await _ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO DD_GENERATELETTER 
              (DD_CRT_PIN, DD_USR_PIN, DD_USR_NAM, DD_LETTERTYPE, DD_EFF_DAT, DD_PRN_DAT)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
            sqlParams);

        // Publish domain event via message bus
        await _publisher.PublishAsync(new { EmployeePin = request.EmployeePin, LetterType = request.LetterType, GeneratedAt = DateTime.UtcNow }, cancellationToken);

        return new GeneratedLetterDto(
            letter.EmployeePin, letter.EmployeeName, letter.SignatoryName,
            letter.LetterType, letter.FinalRating, letter.EffectiveDate, letter.PrintDate);
    }
}
