using AutoMapper;
using MediatR;
using ProjectService.Application.DTOs;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Events;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Application.Commands.Handlers;

public class CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateProjectCommand, ProjectMainDto>
{
    public async Task<ProjectMainDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new ProjectMain
        {
            ProjName = request.ProjName,
            ProjCharterNo = request.ProjCharterNo,
            ProjLeaderId = request.ProjLeaderId,
            ProjTypeId = request.ProjTypeId,
            ProjLocId = request.ProjLocId,
            ProjProcessId = request.ProjProcessId,
            ProjStartDate = request.ProjStartDate,
            ProjEndDate = request.ProjEndDate,
            ProjEstEndDate = request.ProjEstEndDate,
            ProjStatus = 'A',
            ProjRevNo = 1,
            ProjVerNo = 1,
            ProjObjId = request.ProjObjId,
            ProjObjDesc = request.ProjObjDesc,
            ProjTargetProd = request.ProjTargetProd,
            ProjTargetProdRem = request.ProjTargetProdRem,
            ProjNotes = request.ProjNotes,
            ProjLastModifiedOn = DateTime.UtcNow
        };

        project.AddDomainEvent(new ProjectCreatedEvent(project.ProjId, project.ProjName));
        await unitOfWork.ProjectMains.AddAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectMainDto>(project);
    }
}

public class UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateProjectCommand, ProjectMainDto>
{
    public async Task<ProjectMainDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjId} not found.");

        project.ProjName = request.ProjName;
        project.ProjCharterNo = request.ProjCharterNo;
        project.ProjLeaderId = request.ProjLeaderId;
        project.ProjTypeId = request.ProjTypeId;
        project.ProjLocId = request.ProjLocId;
        project.ProjProcessId = request.ProjProcessId;
        project.ProjStartDate = request.ProjStartDate;
        project.ProjEndDate = request.ProjEndDate;
        project.ProjEstEndDate = request.ProjEstEndDate;
        project.ProjObjId = request.ProjObjId;
        project.ProjObjDesc = request.ProjObjDesc;
        project.ProjTargetProd = request.ProjTargetProd;
        project.ProjTargetProdRem = request.ProjTargetProdRem;
        project.ProjNotes = request.ProjNotes;
        project.ProjLastModifiedOn = DateTime.UtcNow;
        project.ProjRevNo++;

        await unitOfWork.ProjectMains.UpdateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectMainDto>(project);
    }
}

public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProjectCommand, bool>
{
    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjId} not found.");

        await unitOfWork.ProjectMains.DeleteAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ChangeProjectStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<ChangeProjectStatusCommand, ProjectMainDto>
{
    public async Task<ProjectMainDto> Handle(ChangeProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjId} not found.");

        var oldStatus = project.ProjStatus;
        project.ProjStatus = request.NewStatus;
        project.ProjLastModifiedOn = DateTime.UtcNow;

        project.AddDomainEvent(new ProjectStatusChangedEvent(project.ProjId, oldStatus, request.NewStatus));

        await unitOfWork.ProjectMains.UpdateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectMainDto>(project);
    }
}

public class HoldProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepository<ProjectHold> holdRepo)
    : IRequestHandler<HoldProjectCommand, ProjectHoldDto>
{
    public async Task<ProjectHoldDto> Handle(HoldProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjId} not found.");

        var hold = new ProjectHold
        {
            ProjHoldProjId = request.ProjId,
            ProjHoldType = 'H',
            ProjHoldDate = DateTime.UtcNow,
            ProjHoldReason = request.Reason,
            ProjHoldUpdatedBy = request.UpdatedBy,
            ProjHoldUpdatedOn = DateTime.UtcNow
        };

        project.ProjStatus = 'H';
        project.AddDomainEvent(new ProjectHeldEvent(project.ProjId, request.Reason));

        await holdRepo.AddAsync(hold, cancellationToken);
        await unitOfWork.ProjectMains.UpdateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectHoldDto>(hold);
    }
}

public class CloseProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CloseProjectCommand, ProjectMainDto>
{
    public async Task<ProjectMainDto> Handle(CloseProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjId} not found.");

        project.ProjStatus = 'C';
        project.ProjClsDate = DateTime.UtcNow;
        project.ProjLastModifiedOn = DateTime.UtcNow;

        project.AddDomainEvent(new ProjectClosedEvent(project.ProjId, project.ProjClsDate.Value));

        await unitOfWork.ProjectMains.UpdateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectMainDto>(project);
    }
}

public class AddProjectMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<AddProjectMemberCommand, ProjectMemberDto>
{
    public async Task<ProjectMemberDto> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var member = new ProjectMember
        {
            ProjMemProjId = request.ProjId,
            ProjMemFuncId = request.FuncId,
            ProjMemEmpSysId = request.EmpSysId
        };

        member.AddDomainEvent(new ProjectMemberAddedEvent(request.ProjId, member.ProjMemId, request.EmpSysId));
        await unitOfWork.ProjectMembers.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectMemberDto>(member);
    }
}

public class RemoveProjectMemberCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveProjectMemberCommand, bool>
{
    public async Task<bool> Handle(RemoveProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await unitOfWork.ProjectMembers.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new KeyNotFoundException($"Member {request.MemberId} not found.");

        member.AddDomainEvent(new ProjectMemberRemovedEvent(member.ProjMemProjId, member.ProjMemId));
        await unitOfWork.ProjectMembers.DeleteAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class AddProjectStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepository<ProjectStatusHistory> statusRepo)
    : IRequestHandler<AddProjectStatusCommand, ProjectStatusHistoryDto>
{
    public async Task<ProjectStatusHistoryDto> Handle(AddProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var status = new ProjectStatusHistory
        {
            ProjStatusProjId = request.ProjId,
            ProjStatusDate = DateTime.UtcNow,
            ProjStatusRem = request.Remarks,
            ProjStatusFile = request.StatusFile,
            ProjStatusRevNo = 1,
            ProjStatusVerNo = 1
        };

        await statusRepo.AddAsync(status, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectStatusHistoryDto>(status);
    }
}
