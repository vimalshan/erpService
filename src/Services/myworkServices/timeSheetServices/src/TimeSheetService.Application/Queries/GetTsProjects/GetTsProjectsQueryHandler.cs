using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetTsProjects;

public class GetTsProjectsQueryHandler : IRequestHandler<GetTsProjectsQuery, IEnumerable<TsProjectDto>>
{
    private readonly ITsProjectRepository _repository;
    private readonly IMapper _mapper;

    public GetTsProjectsQueryHandler(ITsProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TsProjectDto>> Handle(GetTsProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TsProjectDto>>(projects);
    }
}
