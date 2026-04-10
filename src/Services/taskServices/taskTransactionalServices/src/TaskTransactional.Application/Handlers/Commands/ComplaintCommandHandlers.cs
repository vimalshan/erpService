using TaskTransactional.Application.Commands;
using TaskTransactional.Domain.Entities;
using TaskTransactional.Domain.Interfaces;
using MediatR;

namespace TaskTransactional.Application.Handlers.Commands;

// Complaint Main Handlers
public class CreateComplaintMainHandler(IUnitOfWork uow)
    : IRequestHandler<CreateComplaintMainCommand, string>
{
    public async Task<string> Handle(CreateComplaintMainCommand request, CancellationToken ct)
    {
        var entity = ComplaintMain.Create(
            request.UnitCode, request.GroupId, request.GroupName, request.GroupSrc,
            request.GroupDesc, request.BehalfFlg, request.BehalfPin,
            request.RegPin, request.Shift, request.Mail);
        await uow.ComplaintMains.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.CmGroupId;
    }
}

public class UpdateComplaintMainHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateComplaintMainCommand, bool>
{
    public async Task<bool> Handle(UpdateComplaintMainCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintMains.GetByGroupIdAsync(request.GroupId, ct);
        if (entity is null) return false;
        entity.Update(request.GroupName, request.GroupDesc, request.Mail, request.UpdatedBy);
        uow.ComplaintMains.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteComplaintMainHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteComplaintMainCommand, bool>
{
    public async Task<bool> Handle(DeleteComplaintMainCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintMains.GetByGroupIdAsync(request.GroupId, ct);
        if (entity is null) return false;
        uow.ComplaintMains.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// Ticket (Complaint Detail) Handlers
public class CreateTicketHandler(IUnitOfWork uow)
    : IRequestHandler<CreateTicketCommand, decimal>
{
    public async Task<decimal> Handle(CreateTicketCommand request, CancellationToken ct)
    {
        var all = await uow.ComplaintDetails.GetAllAsync(ct);
        var nextId = all.Any() ? all.Max(x => x.CdTicketNum) + 1 : 1;
        var entity = ComplaintDetail.Create(
            nextId, request.GroupId, request.Type, request.Location,
            request.Department, request.Process, request.TargetDate,
            request.Subject, request.Description, request.Ncr);
        await uow.ComplaintDetails.AddAsync(entity, ct);

        // Also create default action record
        var actions = await uow.ComplaintActions.GetAllAsync(ct);
        var nextActionId = actions.Any() ? actions.Max(x => x.CaActionNum) + 1 : 1;
        var action = ComplaintAction.Create(nextActionId, nextId);
        await uow.ComplaintActions.AddAsync(action, ct);

        // Create initial history
        var histories = await uow.ComplaintHistories.GetByActionNumAsync(nextActionId, ct);
        var nextHistId = histories.Any() ? histories.Max(x => x.ChHistoryNum) + 1 : 1;
        var history = ComplaintHistory.Create(nextHistId, nextActionId, 1, "Open", "New Ticket", "O", request.Subject);
        await uow.ComplaintHistories.AddAsync(history, ct);

        await uow.SaveChangesAsync(ct);
        return nextId;
    }
}

public class CloseTicketHandler(IUnitOfWork uow)
    : IRequestHandler<CloseTicketCommand, bool>
{
    public async Task<bool> Handle(CloseTicketCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintDetails.GetByTicketNumAsync(request.TicketNum, ct);
        if (entity is null) return false;
        entity.Close();
        uow.ComplaintDetails.Update(entity);

        // Close associated action
        var action = await uow.ComplaintActions.GetByTaskNumAsync(request.TicketNum, ct);
        if (action is not null)
        {
            action.Close();
            uow.ComplaintActions.Update(action);
        }

        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// Complaint Task Handlers
public class CreateComplaintTaskHandler(IUnitOfWork uow)
    : IRequestHandler<CreateComplaintTaskCommand, decimal>
{
    public async Task<decimal> Handle(CreateComplaintTaskCommand request, CancellationToken ct)
    {
        var tasks = await uow.ComplaintTasks.GetByTicketNumAsync(request.TicketNum, ct);
        var nextId = tasks.Any() ? tasks.Max(x => x.CtTaskNum) + 1 : 1;
        var entity = ComplaintTask.Create(
            nextId, request.TicketNum, request.ScheduleFreq,
            request.ScheduleValue, request.ScheduleTime, request.ScheduleDay);
        await uow.ComplaintTasks.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.CtTaskNum;
    }
}

public class CloseComplaintTaskHandler(IUnitOfWork uow)
    : IRequestHandler<CloseComplaintTaskCommand, bool>
{
    public async Task<bool> Handle(CloseComplaintTaskCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintTasks.GetByTaskNumAsync(request.TaskNum, ct);
        if (entity is null) return false;
        entity.Close(request.UpdatedBy);
        uow.ComplaintTasks.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// Action Handlers
public class CreateActionHandler(IUnitOfWork uow)
    : IRequestHandler<CreateActionCommand, decimal>
{
    public async Task<decimal> Handle(CreateActionCommand request, CancellationToken ct)
    {
        var all = await uow.ComplaintActions.GetAllAsync(ct);
        var nextId = all.Any() ? all.Max(x => x.CaActionNum) + 1 : 1;
        var entity = ComplaintAction.Create(nextId, request.TaskNum);
        await uow.ComplaintActions.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.CaActionNum;
    }
}

public class UpdatePrimaryActionHandler(IUnitOfWork uow)
    : IRequestHandler<UpdatePrimaryActionCommand, bool>
{
    public async Task<bool> Handle(UpdatePrimaryActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.SetPrimaryAction(request.Resp, request.ActBy, request.Solution);
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class UpdateSecondaryActionHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateSecondaryActionCommand, bool>
{
    public async Task<bool> Handle(UpdateSecondaryActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.SetSecondaryAction(request.Resp, request.ActBy, request.Solution, request.EscHrs);
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class UpdateForwardActionHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateForwardActionCommand, bool>
{
    public async Task<bool> Handle(UpdateForwardActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.SetForwardAction(request.Remarks, request.Resp, request.ActBy, request.Solution);
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class UpdateCorrectiveActionHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateCorrectiveActionCommand, bool>
{
    public async Task<bool> Handle(UpdateCorrectiveActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.SetCorrectiveAction(request.ActReq, request.Remarks, request.Resp, request.ActBy, request.Solution);
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CloseActionHandler(IUnitOfWork uow)
    : IRequestHandler<CloseActionCommand, bool>
{
    public async Task<bool> Handle(CloseActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.Close();
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class ReopenActionHandler(IUnitOfWork uow)
    : IRequestHandler<ReopenActionCommand, bool>
{
    public async Task<bool> Handle(ReopenActionCommand request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        if (entity is null) return false;
        entity.Reopen(request.Remarks);
        uow.ComplaintActions.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

// History Handler
public class CreateHistoryHandler(IUnitOfWork uow)
    : IRequestHandler<CreateHistoryCommand, decimal>
{
    public async Task<decimal> Handle(CreateHistoryCommand request, CancellationToken ct)
    {
        var existing = await uow.ComplaintHistories.GetByActionNumAsync(request.ActionNum, ct);
        var nextId = existing.Any() ? existing.Max(x => x.ChHistoryNum) + 1 : 1;
        var nextSerial = existing.Any() ? existing.Max(x => x.ChSerialNum) + 1 : 1;
        var entity = ComplaintHistory.Create(
            nextId, request.ActionNum, nextSerial,
            request.From, request.To, request.ActionType, request.Remarks);
        await uow.ComplaintHistories.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.ChHistoryNum;
    }
}

// Escalation Handlers
public class CreateEscalationHandler(IUnitOfWork uow)
    : IRequestHandler<CreateEscalationCommand, bool>
{
    public async Task<bool> Handle(CreateEscalationCommand request, CancellationToken ct)
    {
        var entity = ComplaintEscalation.Create(
            request.TicketNum, request.LevelNum, request.EscNoHrs, request.UserPin);
        await uow.ComplaintEscalations.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CloseEscalationHandler(IUnitOfWork uow)
    : IRequestHandler<CloseEscalationCommand, bool>
{
    public async Task<bool> Handle(CloseEscalationCommand request, CancellationToken ct)
    {
        var escalations = await uow.ComplaintEscalations.GetByTicketNumAsync(request.TicketNum, ct);
        var entity = escalations.FirstOrDefault(e => e.CeLevelNum == request.LevelNum);
        if (entity is null) return false;
        entity.Close(request.UpdatedBy);
        uow.ComplaintEscalations.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
