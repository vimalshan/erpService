using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using MediatR;

namespace CalendarService.Application.Features.Patterns.Queries;

public record GetPatternByIdQuery(int Id) : IRequest<PatternDto>;
public record GetAllPatternsQuery : IRequest<IEnumerable<PatternDto>>;

public class GetPatternByIdHandler(IPatternRepository repo, IMapper mapper)
    : IRequestHandler<GetPatternByIdQuery, PatternDto>
{
    public async Task<PatternDto> Handle(GetPatternByIdQuery q, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(q.Id, ct)
            ?? throw new PatternNotFoundException(q.Id);
        return mapper.Map<PatternDto>(entity);
    }
}

public class GetAllPatternsHandler(IPatternRepository repo, IMapper mapper)
    : IRequestHandler<GetAllPatternsQuery, IEnumerable<PatternDto>>
{
    public async Task<IEnumerable<PatternDto>> Handle(GetAllPatternsQuery q, CancellationToken ct)
        => mapper.Map<IEnumerable<PatternDto>>(await repo.GetAllAsync(ct));
}
