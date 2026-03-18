using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using MediatR;

namespace CalendarService.Application.Features.Holidays.Queries;

public record GetHolidayByIdQuery(int Id) : IRequest<HolidayDto>;
public record GetAllHolidaysQuery : IRequest<IEnumerable<HolidayDto>>;
public record GetHolidaysByDateRangeQuery(DateTime From, DateTime To) : IRequest<IEnumerable<HolidayDto>>;

public class GetHolidayByIdHandler(IHolidayRepository repo, IMapper mapper)
    : IRequestHandler<GetHolidayByIdQuery, HolidayDto>
{
    public async Task<HolidayDto> Handle(GetHolidayByIdQuery q, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(q.Id, ct)
            ?? throw new HolidayNotFoundException(q.Id);
        return mapper.Map<HolidayDto>(entity);
    }
}

public class GetAllHolidaysHandler(IHolidayRepository repo, IMapper mapper)
    : IRequestHandler<GetAllHolidaysQuery, IEnumerable<HolidayDto>>
{
    public async Task<IEnumerable<HolidayDto>> Handle(GetAllHolidaysQuery q, CancellationToken ct)
        => mapper.Map<IEnumerable<HolidayDto>>(await repo.GetAllAsync(ct));
}

public class GetHolidaysByDateRangeHandler(IHolidayRepository repo, IMapper mapper)
    : IRequestHandler<GetHolidaysByDateRangeQuery, IEnumerable<HolidayDto>>
{
    public async Task<IEnumerable<HolidayDto>> Handle(GetHolidaysByDateRangeQuery q, CancellationToken ct)
        => mapper.Map<IEnumerable<HolidayDto>>(await repo.GetByDateRangeAsync(q.From, q.To, ct));
}
