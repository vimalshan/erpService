using AutoMapper;
using MediatR;
using UtilityService.Application.DTOs;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Queries.GetAllToadPlanSql;

public class GetAllToadPlanSqlQueryHandler : IRequestHandler<GetAllToadPlanSqlQuery, PagedResultDto<ToadPlanSqlDto>>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IMapper _mapper;

    public GetAllToadPlanSqlQueryHandler(IToadPlanSqlRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<ToadPlanSqlDto>> Handle(GetAllToadPlanSqlQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var total = await _repository.GetCountAsync(cancellationToken);

        return new PagedResultDto<ToadPlanSqlDto>
        {
            Items = _mapper.Map<IEnumerable<ToadPlanSqlDto>>(items),
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
