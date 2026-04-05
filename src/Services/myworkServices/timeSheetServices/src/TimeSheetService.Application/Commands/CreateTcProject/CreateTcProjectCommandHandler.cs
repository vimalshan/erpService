using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Commands.CreateTcProject;

public class CreateTcProjectCommandHandler : IRequestHandler<CreateTcProjectCommand, TcProjectDto>
{
    private readonly ITcProjectRepository _repository;
    private readonly IMapper _mapper;

    public CreateTcProjectCommandHandler(ITcProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TcProjectDto> Handle(CreateTcProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new TcProject(
            request.ProjectId,
            request.ProjectName,
            request.CategoryId,
            request.EffectiveDate,
            request.TeamId,
            request.ListAll[0],
            request.ModifiedBy,
            request.OldProjectId);

        await _repository.AddAsync(project, cancellationToken);
        return _mapper.Map<TcProjectDto>(project);
    }
}
