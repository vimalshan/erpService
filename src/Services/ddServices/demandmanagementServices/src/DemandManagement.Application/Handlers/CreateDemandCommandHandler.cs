using MediatR;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Application.Commands;

namespace DemandManagement.Application.Handlers;

public class CreateDemandCommandHandler : IRequestHandler<CreateDemandCommand, long>
{
    private readonly IDemandRepository _demandRepository;

    public CreateDemandCommandHandler(IDemandRepository demandRepository)
    {
        _demandRepository = demandRepository;
    }

    public async Task<long> Handle(CreateDemandCommand command, CancellationToken cancellationToken)
    {
        var demand = new DemandMaster
        {
            DemandType = command.Request.DemandType,
            DepartmentId = command.Request.DepartmentId,
            DemandDescription = command.Request.DemandDescription,
            RequiredDate = command.Request.RequiredDate,
            Priority = command.Request.Priority,
            DemandStatus = "O", // Open
            CreatedBy = command.Request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        return await _demandRepository.AddAsync(demand);
    }
}
