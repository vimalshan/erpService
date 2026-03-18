using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Queries.MeetingTypes;

public class GetAllMeetingTypesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllMeetingTypesQuery, IReadOnlyList<MeetingTypeDto>>
{
    public async Task<IReadOnlyList<MeetingTypeDto>> Handle(GetAllMeetingTypesQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingTypes.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<MeetingTypeDto>>(entities);
    }
}

public class GetActiveMeetingTypesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetActiveMeetingTypesQuery, IReadOnlyList<MeetingTypeDto>>
{
    public async Task<IReadOnlyList<MeetingTypeDto>> Handle(GetActiveMeetingTypesQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingTypes.GetActiveAsync(ct);
        return mapper.Map<IReadOnlyList<MeetingTypeDto>>(entities);
    }
}

public class GetMeetingTypeByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingTypeByIdQuery, MeetingTypeDto?>
{
    public async Task<MeetingTypeDto?> Handle(GetMeetingTypeByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.MeetingTypes.GetByIdAsync(request.Id, ct);
        return entity is null ? null : mapper.Map<MeetingTypeDto>(entity);
    }
}

public class GetMeetingTypeByCodeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingTypeByCodeQuery, MeetingTypeDto?>
{
    public async Task<MeetingTypeDto?> Handle(GetMeetingTypeByCodeQuery request, CancellationToken ct)
    {
        var entity = await uow.MeetingTypes.GetByCodeAsync(request.Code, ct);
        return entity is null ? null : mapper.Map<MeetingTypeDto>(entity);
    }
}
