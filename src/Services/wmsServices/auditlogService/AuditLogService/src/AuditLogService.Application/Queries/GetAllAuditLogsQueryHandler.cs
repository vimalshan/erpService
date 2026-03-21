using AutoMapper;
using AuditLogService.Application.DTOs;
using AuditLogService.Domain.Repositories;
using MediatR;

namespace AuditLogService.Application.Queries;

public class GetAllAuditLogsQueryHandler : IRequestHandler<GetAllAuditLogsQuery, IReadOnlyList<AuditLogDto>>
{
    private readonly IAuditLogRepository _repository;
    private readonly IMapper _mapper;

    public GetAllAuditLogsQueryHandler(IAuditLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AuditLogDto>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var entries = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<AuditLogDto>>(entries);
    }
}
