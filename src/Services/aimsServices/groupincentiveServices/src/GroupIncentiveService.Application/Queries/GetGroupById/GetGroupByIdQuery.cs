using AutoMapper;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Queries.GetGroupById;

public record GetGroupByIdQuery(int GroupId) : IRequest<GroupMasterDto>;

public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, GroupMasterDto>
{
    private readonly IGroupMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetGroupByIdHandler(IGroupMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GroupMasterDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.GroupMaster), request.GroupId);

        return _mapper.Map<GroupMasterDto>(group);
    }
}
