using AutoMapper;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Queries.GetGroupIncentiveById;

public record GetGroupIncentiveByIdQuery(long IncentiveId) : IRequest<GroupIncentiveMainDto>;

public class GetGroupIncentiveByIdHandler : IRequestHandler<GetGroupIncentiveByIdQuery, GroupIncentiveMainDto>
{
    private readonly IGroupIncentiveMainRepository _repository;
    private readonly IMapper _mapper;

    public GetGroupIncentiveByIdHandler(IGroupIncentiveMainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GroupIncentiveMainDto> Handle(GetGroupIncentiveByIdQuery request, CancellationToken cancellationToken)
    {
        var incentive = await _repository.GetByIdWithDetailsAsync(request.IncentiveId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.GroupIncentiveMain), request.IncentiveId);

        return _mapper.Map<GroupIncentiveMainDto>(incentive);
    }
}
