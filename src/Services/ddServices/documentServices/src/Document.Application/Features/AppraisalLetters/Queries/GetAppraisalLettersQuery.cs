using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;

namespace Document.Application.Features.AppraisalLetters.Queries;

public record GetAppraisalLettersQuery(string? LetterType = null) : IRequest<IEnumerable<AppraisalLetterDto>>;

public record GetAppraisalLetterByIdQuery(decimal SerialNo) : IRequest<AppraisalLetterDto?>;

public class GetAppraisalLettersQueryHandler : IRequestHandler<GetAppraisalLettersQuery, IEnumerable<AppraisalLetterDto>>
{
    private readonly IApplicationDbContext _ctx;
    private readonly IMapper _mapper;

    public GetAppraisalLettersQueryHandler(IApplicationDbContext ctx, IMapper mapper)
        => (_ctx, _mapper) = (ctx, mapper);

    public async Task<IEnumerable<AppraisalLetterDto>> Handle(GetAppraisalLettersQuery request, CancellationToken cancellationToken)
    {
        var query = _ctx.AppraisalLetters.AsNoTracking();
        if (!string.IsNullOrEmpty(request.LetterType))
            query = query.Where(l => l.LetterType == request.LetterType);
        var result = await query.OrderByDescending(l => l.SerialNo).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AppraisalLetterDto>>(result);
    }
}

public class GetAppraisalLetterByIdQueryHandler : IRequestHandler<GetAppraisalLetterByIdQuery, AppraisalLetterDto?>
{
    private readonly IApplicationDbContext _ctx;
    private readonly IMapper _mapper;

    public GetAppraisalLetterByIdQueryHandler(IApplicationDbContext ctx, IMapper mapper)
        => (_ctx, _mapper) = (ctx, mapper);

    public async Task<AppraisalLetterDto?> Handle(GetAppraisalLetterByIdQuery request, CancellationToken cancellationToken)
    {
        var letter = await _ctx.AppraisalLetters
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.SerialNo == request.SerialNo, cancellationToken);
        return letter == null ? null : _mapper.Map<AppraisalLetterDto>(letter);
    }
}
