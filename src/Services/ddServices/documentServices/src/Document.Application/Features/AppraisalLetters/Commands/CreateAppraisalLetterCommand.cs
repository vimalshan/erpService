using FluentValidation;
using MediatR;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;
using Document.Domain.Entities;

namespace Document.Application.Features.AppraisalLetters.Commands;

public record CreateAppraisalLetterCommand(
    decimal SerialNo,
    string? LetterType,
    DateTime? FromDate,
    DateTime? EndDate,
    string? Paragraph1,
    string? Paragraph2,
    DateTime? EffectiveDate) : IRequest<AppraisalLetterDto>;

public class CreateAppraisalLetterValidator : AbstractValidator<CreateAppraisalLetterCommand>
{
    public CreateAppraisalLetterValidator()
    {
        RuleFor(x => x.SerialNo).GreaterThan(0);
        RuleFor(x => x.LetterType).MaximumLength(9);
    }
}

public class CreateAppraisalLetterCommandHandler : IRequestHandler<CreateAppraisalLetterCommand, AppraisalLetterDto>
{
    private readonly IApplicationDbContext _ctx;

    public CreateAppraisalLetterCommandHandler(IApplicationDbContext ctx) => _ctx = ctx;

    public async Task<AppraisalLetterDto> Handle(CreateAppraisalLetterCommand request, CancellationToken cancellationToken)
    {
        var letter = AppraisalLetter.Create(
            request.SerialNo, request.LetterType, request.FromDate,
            request.EndDate, request.Paragraph1, request.Paragraph2, request.EffectiveDate);

        await _ctx.AppraisalLetters.AddAsync(letter, cancellationToken);
        await _ctx.SaveChangesAsync(cancellationToken);

        return new AppraisalLetterDto(
            letter.SerialNo, letter.BandCode, letter.LetterType,
            letter.FromDate, letter.EndDate, letter.Paragraph1,
            letter.Paragraph2, letter.Paragraph3, letter.Paragraph4,
            letter.Paragraph5, letter.EffectiveDate, letter.PrintDate);
    }
}
