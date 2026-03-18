using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using Entities = AccountingService.Domain.Entities;
using MediatR;

namespace AccountingService.Application.Features.GlPosting.Commands.PostGlEntry;

public class PostGlEntryCommandHandler : IRequestHandler<PostGlEntryCommand, GlPostingDto>
{
    private readonly IApplicationDbContext _context;

    public PostGlEntryCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<GlPostingDto> Handle(PostGlEntryCommand request, CancellationToken cancellationToken)
    {
        var posting = Entities.GlPosting.Create(
            request.AccountCode, request.PostingDate,
            request.DebitAmount, request.CreditAmount,
            request.ReferenceId, request.Remarks);

        _context.GlPostings.Add(posting);
        await _context.SaveChangesAsync(cancellationToken);

        return new GlPostingDto(posting.PostingId, posting.AccountCode, posting.PostingDate,
            posting.DebitAmount, posting.CreditAmount, posting.ReferenceId, posting.PostingRemarks);
    }
}

