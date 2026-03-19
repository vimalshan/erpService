using MediatR;
using ProblemManagement.Application.DTOs;

namespace ProblemManagement.Application.Commands;

public record CreateProblemCommand : IRequest<ProblemDto>
{
    public long Owner { get; init; }
    public string Description { get; init; } = string.Empty;
    public char? Category { get; init; }
    public string? Impact { get; init; }
    public string? ExpectedResult { get; init; }
    public long UnitId { get; init; }
    public long SiteId { get; init; }
    public long EnteredBy { get; init; }
}

public record UpdateProblemCommand : IRequest<ProblemDto>
{
    public long PrId { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? Impact { get; init; }
    public string? ExpectedResult { get; init; }
    public string? Statement { get; init; }
    public long ModBy { get; init; }
}

public record DeleteProblemCommand(long PrId) : IRequest<bool>;

public record ApproveProblemCommand : IRequest<ProblemApprovalDto>
{
    public long ProblemId { get; init; }
    public long ApprovedBy { get; init; }
    public char Status { get; init; }
    public string? Reason { get; init; }
    public char AudienceFlag { get; init; }
}

public record RecordSolutionCommand : IRequest<ProblemSolutionDto>
{
    public long ProblemId { get; init; }
    public string? Description { get; init; }
    public long EnteredBy { get; init; }
}

public record ApproveSolutionCommand : IRequest<SolutionApprovalDto>
{
    public long SolutionId { get; init; }
    public long ApprovedBy { get; init; }
    public char Status { get; init; }
    public string? Reason { get; init; }
    public char? AudienceFlag { get; init; }
}

public record AddSolutionCommentCommand : IRequest<SolutionCommentDto>
{
    public long SolutionId { get; init; }
    public string Text { get; init; } = string.Empty;
    public long CommentBy { get; init; }
}

public record AddAttachmentCommand : IRequest<ProblemAttachmentDto>
{
    public long ProblemId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public Stream FileContent { get; init; } = Stream.Null;
    public string ContentType { get; init; } = string.Empty;
}
