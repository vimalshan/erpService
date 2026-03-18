using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Application.Commands;

namespace DemandManagement.Application.Handlers;

public class ApproveDemandCommandHandler : IRequestHandler<ApproveDemandCommand, bool>
{
    private readonly IDemandRepository _repository;

    public ApproveDemandCommandHandler(IDemandRepository repository) => _repository = repository;

    public async Task<bool> Handle(ApproveDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = await _repository.GetByIdAsync(request.DemandId);
        if (demand is null) return false;
        demand.Approve(request.ApprovedBy, request.Remarks);
        await _repository.UpdateAsync(demand);
        return true;
    }
}

public class RejectDemandCommandHandler : IRequestHandler<RejectDemandCommand, bool>
{
    private readonly IDemandRepository _repository;

    public RejectDemandCommandHandler(IDemandRepository repository) => _repository = repository;

    public async Task<bool> Handle(RejectDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = await _repository.GetByIdAsync(request.DemandId);
        if (demand is null) return false;
        demand.Reject(request.RejectedBy, request.Remarks);
        await _repository.UpdateAsync(demand);
        return true;
    }
}

public class CompleteDemandCommandHandler : IRequestHandler<CompleteDemandCommand, bool>
{
    private readonly IDemandRepository _repository;

    public CompleteDemandCommandHandler(IDemandRepository repository) => _repository = repository;

    public async Task<bool> Handle(CompleteDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = await _repository.GetByIdAsync(request.DemandId);
        if (demand is null) return false;
        demand.Complete(request.CompletedBy, request.Remarks);
        await _repository.UpdateAsync(demand);
        return true;
    }
}
