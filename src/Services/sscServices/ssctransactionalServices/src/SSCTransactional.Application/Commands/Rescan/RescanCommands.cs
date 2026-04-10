using MediatR;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Application.Commands.Rescan;

public record CreateRescanCommand(long DocId, long AllocationId, string RescanTo, string Remarks) : IRequest<RescanDto>;
public record CompleteRescanCommand(long RescanId, long CompletedBy, string CompletionRemarks, string? FilePath = null) : IRequest<RescanDto>;
