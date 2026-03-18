using LovService.Application.Interfaces;
using LovService.Domain.Exceptions;
using MediatR;
using DomainLovType = LovService.Domain.Entities.LovType;

namespace LovService.Application.Commands.LovType;

public class CreateLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateLovTypeCommand, long>
{
    public async Task<long> Handle(CreateLovTypeCommand request, CancellationToken ct)
    {
        if (await uow.LovTypes.ExistsAsync(request.LovTypeId, ct))
            throw new LovDomainException($"LovType with ID {request.LovTypeId} already exists.");

        var lovType = DomainLovType.Create(request.LovTypeId, request.LovTypeName);
        await uow.LovTypes.AddAsync(lovType, ct);
        await uow.SaveChangesAsync(ct);
        return lovType.LovTypeId;
    }
}

public class UpdateLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateLovTypeCommand, bool>
{
    public async Task<bool> Handle(UpdateLovTypeCommand request, CancellationToken ct)
    {
        var lovType = await uow.LovTypes.GetByIdAsync(request.LovTypeId, ct)
            ?? throw new LovNotFoundException("LovType", request.LovTypeId);

        lovType.UpdateName(request.LovTypeName);
        uow.LovTypes.Update(lovType);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteLovTypeCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteLovTypeCommand, bool>
{
    public async Task<bool> Handle(DeleteLovTypeCommand request, CancellationToken ct)
    {
        var lovType = await uow.LovTypes.GetByIdAsync(request.LovTypeId, ct)
            ?? throw new LovNotFoundException("LovType", request.LovTypeId);

        uow.LovTypes.Delete(lovType);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
