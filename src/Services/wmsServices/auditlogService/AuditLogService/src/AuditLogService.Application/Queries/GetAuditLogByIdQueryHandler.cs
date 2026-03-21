using AutoMapper;
using AuditLogService.Application.DTOs;
using AuditLogService.Domain.Repositories;
using MediatR;

namespace AuditLogService.Application.Queries;

public class GetAuditLogByIdQueryHandler : IRequestHandler<GetAuditLogByIdQuery, AuditLogDto?>
{
    private readonly IAuditLogRepository _repository;
    private readonly IMapper _mapper;

    public GetAuditLogByIdQueryHandler(IAuditLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuditLogDto?> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entry is null ? null : _mapper.Map<AuditLogDto>(entry);
    }
}
