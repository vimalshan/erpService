using AutoMapper;
using AuditLogService.Application.DTOs;
using AuditLogService.Domain.Entities;
using AuditLogService.Domain.Repositories;
using MediatR;

namespace AuditLogService.Application.Commands;

public class CreateAuditLogCommandHandler : IRequestHandler<CreateAuditLogCommand, AuditLogDto>
{
    private readonly IAuditLogRepository _repository;
    private readonly IMapper _mapper;

    public CreateAuditLogCommandHandler(IAuditLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuditLogDto> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        var entry = AuditLogEntry.Create(
            request.TableName,
            request.RecordId,
            request.Action,
            request.ChangedBy,
            request.OldValues,
            request.NewValues);

        var created = await _repository.AddAsync(entry, cancellationToken);
        return _mapper.Map<AuditLogDto>(created);
    }
}
