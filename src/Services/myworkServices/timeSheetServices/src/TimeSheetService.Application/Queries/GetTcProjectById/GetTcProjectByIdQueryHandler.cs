using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetTcProjectById;

public class GetTcProjectByIdQueryHandler : IRequestHandler<GetTcProjectByIdQuery, TcProjectDto?>
{
    private readonly ITcProjectRepository _repository;
    private readonly IMapper _mapper;

    public GetTcProjectByIdQueryHandler(ITcProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TcProjectDto?> Handle(GetTcProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        return project is null ? null : _mapper.Map<TcProjectDto>(project);
    }
}
