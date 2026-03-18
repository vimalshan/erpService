using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;

namespace Document.Application.Features.Signatories.Queries;

public record GetSignatoriesQuery(bool ActiveOnly = true) : IRequest<IEnumerable<SignatoryDto>>;

public record GetSignatoryByIdQuery(decimal SignatoryNumber) : IRequest<SignatoryDto?>;

public class GetSignatoriesQueryHandler : IRequestHandler<GetSignatoriesQuery, IEnumerable<SignatoryDto>>
{
    private readonly IApplicationDbContext _ctx;
    private readonly IMapper _mapper;

    public GetSignatoriesQueryHandler(IApplicationDbContext ctx, IMapper mapper)
        => (_ctx, _mapper) = (ctx, mapper);

    public async Task<IEnumerable<SignatoryDto>> Handle(GetSignatoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _ctx.Signatories.AsNoTracking();
        if (request.ActiveOnly) query = query.Where(s => s.LiveFlag == "Y");
        var result = await query.OrderBy(s => s.SignatoryNumber).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SignatoryDto>>(result);
    }
}

public class GetSignatoryByIdQueryHandler : IRequestHandler<GetSignatoryByIdQuery, SignatoryDto?>
{
    private readonly IApplicationDbContext _ctx;
    private readonly IMapper _mapper;

    public GetSignatoryByIdQueryHandler(IApplicationDbContext ctx, IMapper mapper)
        => (_ctx, _mapper) = (ctx, mapper);

    public async Task<SignatoryDto?> Handle(GetSignatoryByIdQuery request, CancellationToken cancellationToken)
    {
        var signatory = await _ctx.Signatories
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SignatoryNumber == request.SignatoryNumber, cancellationToken);
        return signatory == null ? null : _mapper.Map<SignatoryDto>(signatory);
    }
}
