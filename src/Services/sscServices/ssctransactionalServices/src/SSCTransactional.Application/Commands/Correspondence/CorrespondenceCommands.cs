using MediatR;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Application.Commands.Correspondence;

public record CreateCorrespondenceCommand(
    long DocId, long AllocationId, long HoldCategory, long HoldType,
    string HoldRemarks, long HoldBy, decimal? HoldNature = null) : IRequest<CorrespondenceDto>;

public record ReleaseCorrespondenceCommand(long CorrespondenceId, long ReleasedBy, string ReleaseRemarks) : IRequest<CorrespondenceDto>;
