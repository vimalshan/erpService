using MediatR;
using ProblemManagement.Application.DTOs;

namespace ProblemManagement.Application.Queries;

public record GetProblemByIdQuery(long PrId) : IRequest<ProblemDto?>;

public record GetProblemsByStatusQuery(char Status) : IRequest<IReadOnlyList<ProblemDto>>;

public record GetAllProblemsQuery : IRequest<IReadOnlyList<ProblemDto>>;

public record GetSolutionsByProblemQuery(long ProblemId) : IRequest<IReadOnlyList<ProblemSolutionDto>>;

public record GetCommentsBySolutionQuery(long SolutionId) : IRequest<IReadOnlyList<SolutionCommentDto>>;

public record GetProblemFunctionsQuery : IRequest<IReadOnlyList<ProblemFunctionDto>>;

public record GetProblemImpactsQuery : IRequest<IReadOnlyList<ProblemImpactDto>>;

public record GetAttachmentsByProblemQuery(long ProblemId) : IRequest<IReadOnlyList<ProblemAttachmentDto>>;
