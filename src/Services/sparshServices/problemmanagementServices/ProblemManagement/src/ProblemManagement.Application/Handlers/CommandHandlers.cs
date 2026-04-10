using AutoMapper;
using MediatR;
using ProblemManagement.Application.Commands;
using ProblemManagement.Application.DTOs;
using ProblemManagement.Domain.Entities;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Application.Handlers;

public class CreateProblemHandler(
    IProblemRepository repo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<CreateProblemCommand, ProblemDto>
{
    public async Task<ProblemDto> Handle(CreateProblemCommand request, CancellationToken ct)
    {
        var problem = ProblemMain.Create(
            request.Owner, request.EnteredBy, request.Description,
            request.Category, request.Impact, request.ExpectedResult,
            request.UnitId, request.SiteId);

        await repo.AddAsync(problem, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProblemDto>(problem);
    }
}

public class UpdateProblemHandler(
    IProblemRepository repo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<UpdateProblemCommand, ProblemDto>
{
    public async Task<ProblemDto> Handle(UpdateProblemCommand request, CancellationToken ct)
    {
        var problem = await repo.GetByIdAsync(request.PrId, ct)
            ?? throw new KeyNotFoundException($"Problem {request.PrId} not found.");

        problem.PrDescription = request.Description;
        problem.PrImpact = request.Impact;
        problem.PrExpResult = request.ExpectedResult;
        problem.PrStatement = request.Statement;
        problem.PrModBy = request.ModBy;
        problem.PrModOn = DateTime.UtcNow;

        await repo.UpdateAsync(problem, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProblemDto>(problem);
    }
}

public class DeleteProblemHandler(
    IProblemRepository repo,
    IUnitOfWork uow) : IRequestHandler<DeleteProblemCommand, bool>
{
    public async Task<bool> Handle(DeleteProblemCommand request, CancellationToken ct)
    {
        await repo.DeleteAsync(request.PrId, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class ApproveProblemHandler(
    IProblemRepository problemRepo,
    IProblemApprovalRepository approvalRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<ApproveProblemCommand, ProblemApprovalDto>
{
    public async Task<ProblemApprovalDto> Handle(ApproveProblemCommand request, CancellationToken ct)
    {
        var problem = await problemRepo.GetByIdAsync(request.ProblemId, ct)
            ?? throw new KeyNotFoundException($"Problem {request.ProblemId} not found.");

        var approval = new ProblemApproval
        {
            PrAppPrId = request.ProblemId,
            PrAppBy = request.ApprovedBy,
            PrAppOn = DateTime.UtcNow,
            PrAppStatus = request.Status,
            PrAppReason = request.Reason,
            PrAppAudFlag = request.AudienceFlag
        };

        await approvalRepo.AddAsync(approval, ct);

        if (request.Status == "A")
            problem.Approve(request.ApprovedBy, request.Reason, request.AudienceFlag);
        else
            problem.Reject(request.ApprovedBy, request.Reason);

        await problemRepo.UpdateAsync(problem, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProblemApprovalDto>(approval);
    }
}

public class RecordSolutionHandler(
    IProblemRepository problemRepo,
    IProblemSolutionRepository solutionRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<RecordSolutionCommand, ProblemSolutionDto>
{
    public async Task<ProblemSolutionDto> Handle(RecordSolutionCommand request, CancellationToken ct)
    {
        _ = await problemRepo.GetByIdAsync(request.ProblemId, ct)
            ?? throw new KeyNotFoundException($"Problem {request.ProblemId} not found.");

        var solution = new ProblemSolution
        {
            SolPrId = request.ProblemId,
            SolDescription = request.Description,
            SolEnteredBy = request.EnteredBy,
            SolEnteredOn = DateTime.UtcNow
        };

        await solutionRepo.AddAsync(solution, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProblemSolutionDto>(solution);
    }
}

public class ApproveSolutionHandler(
    ISolutionApprovalRepository approvalRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<ApproveSolutionCommand, SolutionApprovalDto>
{
    public async Task<SolutionApprovalDto> Handle(ApproveSolutionCommand request, CancellationToken ct)
    {
        var approval = new SolutionApproval
        {
            SolAppSolId = request.SolutionId,
            SolAppBy = request.ApprovedBy,
            SolAppOn = DateTime.UtcNow,
            SolAppStatus = request.Status,
            SolAppReason = request.Reason,
            SolAppAudFlag = request.AudienceFlag
        };

        await approvalRepo.AddAsync(approval, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<SolutionApprovalDto>(approval);
    }
}

public class AddSolutionCommentHandler(
    ISolutionCommentRepository commentRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<AddSolutionCommentCommand, SolutionCommentDto>
{
    public async Task<SolutionCommentDto> Handle(AddSolutionCommentCommand request, CancellationToken ct)
    {
        var comment = new SolutionComment
        {
            SolCommentSolId = request.SolutionId,
            SolCommentText = request.Text,
            SolCommentBy = request.CommentBy,
            SolCommentOn = DateTime.UtcNow
        };

        await commentRepo.AddAsync(comment, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<SolutionCommentDto>(comment);
    }
}

public class AddAttachmentHandler(
    IProblemAttachmentRepository attachmentRepo,
    IBlobStorageService blobService,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<AddAttachmentCommand, ProblemAttachmentDto>
{
    public async Task<ProblemAttachmentDto> Handle(AddAttachmentCommand request, CancellationToken ct)
    {
        var blobUrl = await blobService.UploadAsync(
            "problem-attachments", request.FileName, request.FileContent, request.ContentType, ct);

        var attachment = new ProblemAttachment
        {
            PratPrId = request.ProblemId,
            PratFileName = blobUrl,
            PratEnteredOn = DateTime.UtcNow
        };

        await attachmentRepo.AddAsync(attachment, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProblemAttachmentDto>(attachment);
    }
}
