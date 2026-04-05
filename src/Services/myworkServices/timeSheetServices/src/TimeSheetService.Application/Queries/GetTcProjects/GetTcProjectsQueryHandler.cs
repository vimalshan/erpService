using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetTcProjects;

public class GetTcProjectsQueryHandler : IRequestHandler<GetTcProjectsQuery, IEnumerable<TcProjectDto>>
{
    private readonly ITcProjectRepository _repository;
    private readonly IMapper _mapper;

    public GetTcProjectsQueryHandler(ITcProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TcProjectDto>> Handle(GetTcProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TcProjectDto>>(projects);
    }
}
