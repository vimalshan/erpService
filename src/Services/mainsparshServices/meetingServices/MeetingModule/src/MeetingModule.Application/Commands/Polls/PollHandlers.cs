using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Entities;
using MeetingModule.Domain.Exceptions;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Commands.Polls;

public class CreatePollHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreatePollCommand, PollDetailDto>
{
    public async Task<PollDetailDto> Handle(CreatePollCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = PollDetail.Create(dto.MeetingId, dto.PollQuestion, dto.PollType, request.UserId);
        await uow.PollDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<PollDetailDto>(entity);
    }
}

public class UpdatePollHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdatePollCommand, PollDetailDto>
{
    public async Task<PollDetailDto> Handle(UpdatePollCommand request, CancellationToken ct)
    {
        var entity = await uow.PollDetails.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(PollDetail), request.Id);
        entity.Update(request.Dto.PollQuestion, request.Dto.PollType, request.UserId);
        await uow.PollDetails.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<PollDetailDto>(entity);
    }
}

public class ClosePollHandler(IUnitOfWork uow)
    : IRequestHandler<ClosePollCommand, Unit>
{
    public async Task<Unit> Handle(ClosePollCommand request, CancellationToken ct)
    {
        var entity = await uow.PollDetails.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException(nameof(PollDetail), request.Id);
        entity.Close(request.UserId);
        await uow.PollDetails.UpdateAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
