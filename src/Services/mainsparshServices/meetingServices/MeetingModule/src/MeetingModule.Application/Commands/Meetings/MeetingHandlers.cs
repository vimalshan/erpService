using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Entities;
using MeetingModule.Domain.Exceptions;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Commands.Meetings;

public class CreateMeetingHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateMeetingCommand, MeetingScheduleDto>
{
    public async Task<MeetingScheduleDto> Handle(CreateMeetingCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = MeetingSchedule.Create(
            dto.MeetTypeId, dto.MeetingTitle, dto.MeetingDate,
            dto.MeetingLocation, dto.MeetingDuration, dto.OrganizerId,
            dto.Notes, request.UserId);

        await uow.MeetingSchedules.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<MeetingScheduleDto>(entity);
    }
}

public class UpdateMeetingHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateMeetingCommand, MeetingScheduleDto>
{
    public async Task<MeetingScheduleDto> Handle(UpdateMeetingCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingSchedules.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingSchedule), request.Id);

        var dto = request.Dto;
        entity.Update(dto.MeetingTitle, dto.MeetingDate, dto.MeetingLocation, dto.MeetingDuration, dto.Notes, request.UserId);
        await uow.MeetingSchedules.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<MeetingScheduleDto>(entity);
    }
}

public class StartMeetingHandler(IUnitOfWork uow)
    : IRequestHandler<StartMeetingCommand, Unit>
{
    public async Task<Unit> Handle(StartMeetingCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingSchedules.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingSchedule), request.Id);
        entity.Start(request.UserId);
        await uow.MeetingSchedules.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CompleteMeetingHandler(IUnitOfWork uow)
    : IRequestHandler<CompleteMeetingCommand, Unit>
{
    public async Task<Unit> Handle(CompleteMeetingCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingSchedules.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingSchedule), request.Id);
        entity.Complete(request.UserId);
        await uow.MeetingSchedules.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CancelMeetingHandler(IUnitOfWork uow)
    : IRequestHandler<CancelMeetingCommand, Unit>
{
    public async Task<Unit> Handle(CancelMeetingCommand request, CancellationToken ct)
    {
        var entity = await uow.MeetingSchedules.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(MeetingSchedule), request.Id);
        entity.Cancel(request.UserId);
        await uow.MeetingSchedules.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
