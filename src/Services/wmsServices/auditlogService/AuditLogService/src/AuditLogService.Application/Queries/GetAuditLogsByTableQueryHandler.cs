using AutoMapper;
using AuditLogService.Application.DTOs;
using AuditLogService.Domain.Repositories;
using MediatR;

namespace AuditLogService.Application.Queries;

public class GetAuditLogsByTableQueryHandler : IRequestHandler<GetAuditLogsByTableQuery, IReadOnlyList<AuditLogDto>>
{
    private readonly IAuditLogRepository _repository;
    private readonly IMapper _mapper;

    public GetAuditLogsByTableQueryHandler(IAuditLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AuditLogDto>> Handle(GetAuditLogsByTableQuery request, CancellationToken cancellationToken)
    {
        var entries = await _repository.GetByTableNameAsync(request.TableName, cancellationToken);
        return _mapper.Map<IReadOnlyList<AuditLogDto>>(entries);
    }
}
