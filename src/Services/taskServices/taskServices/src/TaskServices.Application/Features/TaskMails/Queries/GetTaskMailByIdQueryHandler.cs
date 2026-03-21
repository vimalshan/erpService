using AutoMapper;
using MediatR;
using TaskServices.Application.DTOs;
using TaskServices.Domain.Repositories;

namespace TaskServices.Application.Features.TaskMails.Queries;

public class GetTaskMailByIdQueryHandler : IRequestHandler<GetTaskMailByIdQuery, TaskMailDto?>
{
    private readonly ITaskMailRepository _repository;
    private readonly IMapper _mapper;

    public GetTaskMailByIdQueryHandler(ITaskMailRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TaskMailDto?> Handle(GetTaskMailByIdQuery request, CancellationToken cancellationToken)
    {
        var taskMail = await _repository.GetByIdAsync(request.MID, cancellationToken);
        return taskMail is null ? null : _mapper.Map<TaskMailDto>(taskMail);
    }
}
