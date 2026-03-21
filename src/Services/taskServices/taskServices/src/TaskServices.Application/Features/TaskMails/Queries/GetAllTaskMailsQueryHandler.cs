using AutoMapper;
using MediatR;
using TaskServices.Application.DTOs;
using TaskServices.Domain.Repositories;

namespace TaskServices.Application.Features.TaskMails.Queries;

public class GetAllTaskMailsQueryHandler : IRequestHandler<GetAllTaskMailsQuery, IReadOnlyList<TaskMailDto>>
{
    private readonly ITaskMailRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTaskMailsQueryHandler(ITaskMailRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaskMailDto>> Handle(GetAllTaskMailsQuery request, CancellationToken cancellationToken)
    {
        var taskMails = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TaskMailDto>>(taskMails);
    }
}
