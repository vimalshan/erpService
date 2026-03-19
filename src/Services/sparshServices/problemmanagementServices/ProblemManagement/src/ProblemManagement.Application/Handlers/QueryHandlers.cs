using AutoMapper;
using MediatR;
using ProblemManagement.Application.DTOs;
using ProblemManagement.Application.Queries;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Application.Handlers;

public class GetProblemByIdHandler(
    IProblemRepository repo,
    IMapper mapper) : IRequestHandler<GetProblemByIdQuery, ProblemDto?>
{
    public async Task<ProblemDto?> Handle(GetProblemByIdQuery request, CancellationToken ct)
    {
        var problem = await repo.GetByIdAsync(request.PrId, ct);
        return problem is null ? null : mapper.Map<ProblemDto>(problem);
    }
}

public class GetProblemsByStatusHandler(
    IProblemRepository repo,
    IMapper mapper) : IRequestHandler<GetProblemsByStatusQuery, IReadOnlyList<ProblemDto>>
{
    public async Task<IReadOnlyList<ProblemDto>> Handle(GetProblemsByStatusQuery request, CancellationToken ct)
    {
        var problems = await repo.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<ProblemDto>>(problems);
    }
}

public class GetAllProblemsHandler(
    IProblemRepository repo,
    IMapper mapper) : IRequestHandler<GetAllProblemsQuery, IReadOnlyList<ProblemDto>>
{
    public async Task<IReadOnlyList<ProblemDto>> Handle(GetAllProblemsQuery request, CancellationToken ct)
    {
        var problems = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ProblemDto>>(problems);
    }
}

public class GetSolutionsByProblemHandler(
    IProblemSolutionRepository repo,
    IMapper mapper) : IRequestHandler<GetSolutionsByProblemQuery, IReadOnlyList<ProblemSolutionDto>>
{
    public async Task<IReadOnlyList<ProblemSolutionDto>> Handle(GetSolutionsByProblemQuery request, CancellationToken ct)
    {
        var solutions = await repo.GetByProblemIdAsync(request.ProblemId, ct);
        return mapper.Map<IReadOnlyList<ProblemSolutionDto>>(solutions);
    }
}

public class GetCommentsBySolutionHandler(
    ISolutionCommentRepository repo,
    IMapper mapper) : IRequestHandler<GetCommentsBySolutionQuery, IReadOnlyList<SolutionCommentDto>>
{
    public async Task<IReadOnlyList<SolutionCommentDto>> Handle(GetCommentsBySolutionQuery request, CancellationToken ct)
    {
        var comments = await repo.GetBySolutionIdAsync(request.SolutionId, ct);
        return mapper.Map<IReadOnlyList<SolutionCommentDto>>(comments);
    }
}

public class GetProblemFunctionsHandler(
    IProblemFunctionRepository repo,
    IMapper mapper) : IRequestHandler<GetProblemFunctionsQuery, IReadOnlyList<ProblemFunctionDto>>
{
    public async Task<IReadOnlyList<ProblemFunctionDto>> Handle(GetProblemFunctionsQuery request, CancellationToken ct)
    {
        var functions = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ProblemFunctionDto>>(functions);
    }
}

public class GetProblemImpactsHandler(
    IProblemImpactRepository repo,
    IMapper mapper) : IRequestHandler<GetProblemImpactsQuery, IReadOnlyList<ProblemImpactDto>>
{
    public async Task<IReadOnlyList<ProblemImpactDto>> Handle(GetProblemImpactsQuery request, CancellationToken ct)
    {
        var impacts = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ProblemImpactDto>>(impacts);
    }
}

public class GetAttachmentsByProblemHandler(
    IProblemAttachmentRepository repo,
    IMapper mapper) : IRequestHandler<GetAttachmentsByProblemQuery, IReadOnlyList<ProblemAttachmentDto>>
{
    public async Task<IReadOnlyList<ProblemAttachmentDto>> Handle(GetAttachmentsByProblemQuery request, CancellationToken ct)
    {
        var attachments = await repo.GetByProblemIdAsync(request.ProblemId, ct);
        return mapper.Map<IReadOnlyList<ProblemAttachmentDto>>(attachments);
    }
}
