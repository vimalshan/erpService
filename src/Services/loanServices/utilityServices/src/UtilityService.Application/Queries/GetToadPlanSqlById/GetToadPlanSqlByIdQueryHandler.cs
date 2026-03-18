using AutoMapper;
using MediatR;
using UtilityService.Application.DTOs;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Queries.GetToadPlanSqlById;

public class GetToadPlanSqlByIdQueryHandler : IRequestHandler<GetToadPlanSqlByIdQuery, ToadPlanSqlDto?>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IMapper _mapper;

    public GetToadPlanSqlByIdQueryHandler(IToadPlanSqlRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ToadPlanSqlDto?> Handle(GetToadPlanSqlByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<ToadPlanSqlDto>(entity);
    }
}
