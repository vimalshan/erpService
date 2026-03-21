using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using MediatR;

namespace ArchiveService.Application.Features.ServiceOrders.Commands;

public class CreateServiceOrderHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateServiceOrderCommand, string>
{
    public async Task<string> Handle(CreateServiceOrderCommand cmd, CancellationToken ct)
    {
        var order = ArchivedServiceOrder.Create(
            cmd.SernoDell, cmd.Branch, cmd.SapLogin, cmd.PostingDate,
            cmd.SapId, cmd.Sla, cmd.ProductId, cmd.ServiceTag,
            cmd.RelatedCase, cmd.Lob, cmd.CallStatus, cmd.CurrentRc,
            cmd.EngineerId, cmd.EngineerName, cmd.EngMobNo,
            cmd.OrgName, cmd.CustomerName, cmd.ContactNo,
            cmd.Address, cmd.AltContactNo, cmd.DispatchDate,
            cmd.CustEtaDate, cmd.PartEtaDate, cmd.TechSupName,
            cmd.Dsp, cmd.ProblemDescription, cmd.LongDescription,
            cmd.ReasonCode, cmd.Activity, cmd.OnsiteDate,
            cmd.CompletedDate, cmd.Flag, cmd.EnteredBy);

        await unitOfWork.ServiceOrders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return order.SernoDell;
    }
}

public class UpdateServiceOrderStatusHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateServiceOrderStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateServiceOrderStatusCommand cmd, CancellationToken ct)
    {
        var order = await unitOfWork.ServiceOrders.GetByIdAsync(cmd.SernoDell, ct);
        if (order is null) return false;

        order.UpdateStatus(cmd.CallStatus, cmd.ReasonCode, cmd.ChangedBy);
        await unitOfWork.ServiceOrders.UpdateAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteServiceOrderHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteServiceOrderCommand, bool>
{
    public async Task<bool> Handle(DeleteServiceOrderCommand cmd, CancellationToken ct)
    {
        var order = await unitOfWork.ServiceOrders.GetByIdAsync(cmd.SernoDell, ct);
        if (order is null) return false;

        await unitOfWork.ServiceOrders.DeleteAsync(cmd.SernoDell, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
