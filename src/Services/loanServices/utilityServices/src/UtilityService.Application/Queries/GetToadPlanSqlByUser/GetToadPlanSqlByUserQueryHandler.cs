using AutoMapper;
using MediatR;
using UtilityService.Application.DTOs;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Queries.GetToadPlanSqlByUser;

public class GetToadPlanSqlByUserQueryHandler : IRequestHandler<GetToadPlanSqlByUserQuery, IEnumerable<ToadPlanSqlDto>>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IMapper _mapper;

    public GetToadPlanSqlByUserQueryHandler(IToadPlanSqlRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ToadPlanSqlDto>> Handle(GetToadPlanSqlByUserQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByUsernameAsync(request.Username, cancellationToken);
        return _mapper.Map<IEnumerable<ToadPlanSqlDto>>(entities);
    }
}
