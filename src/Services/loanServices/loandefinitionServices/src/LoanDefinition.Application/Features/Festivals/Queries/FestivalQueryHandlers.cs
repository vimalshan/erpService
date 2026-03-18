using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Repositories;
using MediatR;

namespace LoanDefinition.Application.Features.Festivals.Queries;

public class GetAllFestivalsQueryHandler(ILoanFestivalRepository repository, IMapper mapper)
    : IRequestHandler<GetAllFestivalsQuery, IReadOnlyList<LoanFestivalDto>>
{
    public async Task<IReadOnlyList<LoanFestivalDto>> Handle(GetAllFestivalsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<LoanFestivalDto>>(entities);
    }
}

public class GetFestivalByIdQueryHandler(ILoanFestivalRepository repository, IMapper mapper)
    : IRequestHandler<GetFestivalByIdQuery, LoanFestivalDto?>
{
    public async Task<LoanFestivalDto?> Handle(GetFestivalByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.FestivalId, cancellationToken);
        return entity is null ? null : mapper.Map<LoanFestivalDto>(entity);
    }
}

public class GetActiveFestivalsQueryHandler(ILoanFestivalRepository repository, IMapper mapper)
    : IRequestHandler<GetActiveFestivalsQuery, IReadOnlyList<LoanFestivalDto>>
{
    public async Task<IReadOnlyList<LoanFestivalDto>> Handle(GetActiveFestivalsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetActiveFestivalsAsync(request.AsOfDate, cancellationToken);
        return mapper.Map<IReadOnlyList<LoanFestivalDto>>(entities);
    }
}
