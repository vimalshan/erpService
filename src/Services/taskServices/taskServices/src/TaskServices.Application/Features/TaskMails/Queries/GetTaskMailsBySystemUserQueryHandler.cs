using AutoMapper;
using MediatR;
using TaskServices.Application.DTOs;
using TaskServices.Domain.Repositories;

namespace TaskServices.Application.Features.TaskMails.Queries;

public class GetTaskMailsBySystemUserQueryHandler : IRequestHandler<GetTaskMailsBySystemUserQuery, IReadOnlyList<TaskMailDto>>
{
    private readonly ITaskMailRepository _repository;
    private readonly IMapper _mapper;

    public GetTaskMailsBySystemUserQueryHandler(ITaskMailRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaskMailDto>> Handle(GetTaskMailsBySystemUserQuery request, CancellationToken cancellationToken)
    {
        var taskMails = await _repository.GetBySystemUserIdAsync(request.SYSID, cancellationToken);
        return _mapper.Map<IReadOnlyList<TaskMailDto>>(taskMails);
    }
}
