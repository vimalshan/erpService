using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using MediatR;

namespace CalendarService.Application.Features.Shifts.Queries;

public record GetShiftByIdQuery(int Id) : IRequest<ShiftDto>;
public record GetAllShiftsQuery : IRequest<IEnumerable<ShiftDto>>;

public class GetShiftByIdHandler(IShiftRepository repo, IMapper mapper)
    : IRequestHandler<GetShiftByIdQuery, ShiftDto>
{
    public async Task<ShiftDto> Handle(GetShiftByIdQuery q, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(q.Id, ct)
            ?? throw new ShiftNotFoundException(q.Id);
        return mapper.Map<ShiftDto>(entity);
    }
}

public class GetAllShiftsHandler(IShiftRepository repo, IMapper mapper)
    : IRequestHandler<GetAllShiftsQuery, IEnumerable<ShiftDto>>
{
    public async Task<IEnumerable<ShiftDto>> Handle(GetAllShiftsQuery q, CancellationToken ct)
        => mapper.Map<IEnumerable<ShiftDto>>(await repo.GetAllAsync(ct));
}
