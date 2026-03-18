using LovService.Application.Interfaces;
using LovService.Domain.Exceptions;
using MediatR;
using DomainLovType = LovService.Domain.Entities.LovType;
using DomainLovMaster = LovService.Domain.Entities.LovMaster;

namespace LovService.Application.Commands.LovMaster;

public class CreateLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovMasterCommand, long>
{
    public async Task<long> Handle(CreateLovMasterCommand request, CancellationToken ct)
    {
        if (!await uow.LovTypes.ExistsAsync(request.LovTypeId, ct))
            throw new LovNotFoundException("LovType", request.LovTypeId);

        if (await uow.LovMasters.ExistsAsync(request.LovId, ct))
            throw new LovDomainException($"LovMaster with ID {request.LovId} already exists.");

        var master = DomainLovMaster.Create(request.LovId, request.LovTypeId, request.LovName, request.UpdatedBy);
        await uow.LovMasters.AddAsync(master, ct);
        await uow.SaveChangesAsync(ct);
        return master.LovId;
    }
}

public class UpdateLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovMasterCommand, bool>
{
    public async Task<bool> Handle(UpdateLovMasterCommand request, CancellationToken ct)
    {
        var master = await uow.LovMasters.GetByIdAsync(request.LovId, ct)
            ?? throw new LovNotFoundException("LovMaster", request.LovId);

        master.Update(request.LovName, request.UpdatedBy);
        uow.LovMasters.Update(master);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteLovMasterCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovMasterCommand, bool>
{
    public async Task<bool> Handle(DeleteLovMasterCommand request, CancellationToken ct)
    {
        var master = await uow.LovMasters.GetByIdAsync(request.LovId, ct)
            ?? throw new LovNotFoundException("LovMaster", request.LovId);

        uow.LovMasters.Delete(master);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
