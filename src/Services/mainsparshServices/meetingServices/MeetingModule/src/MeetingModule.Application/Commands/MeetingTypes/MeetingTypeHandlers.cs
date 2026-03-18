using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Entities;
using MeetingModule.Domain.Exceptions;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Commands.MeetingTypes;

public class CreateMeetingTypeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateMeetingTypeCommand, MeetingTypeDto>
{
    public async Task<MeetingTypeDto> Handle(CreateMeetingTypeCommand request, CancellationToken ct)
    {
        var entity = MeetingType.Create(request.Dto.MeetTypeCode, request.Dto.MeetTypeName, request.Dto.MeetTypeDesc, request.UserId);
        await uow.MeetingTypes.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<MeetingTypeDto>(entity);
    }
}

public class UpdateMeetingTypeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateMeetingTypeCommand, MeetingTypeDto>
{
    public async Task<MeetingTypeDto> Handle(UpdateMeetingTypeCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingTypes.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingType), request.Id);
        entity.Update(request.Dto.MeetTypeName, request.Dto.MeetTypeDesc, request.UserId);
        await uow.MeetingTypes.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<MeetingTypeDto>(entity);
    }
}

public class ActivateMeetingTypeHandler(IUnitOfWork uow)
    : IRequestHandler<ActivateMeetingTypeCommand, Unit>
{
    public async Task<Unit> Handle(ActivateMeetingTypeCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingTypes.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingType), request.Id);
        entity.Activate(request.UserId);
        await uow.MeetingTypes.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class DeactivateMeetingTypeHandler(IUnitOfWork uow)
    : IRequestHandler<DeactivateMeetingTypeCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateMeetingTypeCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingTypes.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingType), request.Id);
        entity.Deactivate(request.UserId);
        await uow.MeetingTypes.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
