namespace OrderScheduleService.Application.QueryHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Queries;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Domain.Interfaces;

public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ScheduleDto?>
{
    private readonly IScheduleRepository _repository;
    private readonly IMapper _mapper;

    public GetScheduleByIdQueryHandler(IScheduleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ScheduleDto?> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        return _mapper.Map<ScheduleDto>(schedule);
    }
}

public class GetSchedulesByItemQueryHandler : IRequestHandler<GetSchedulesByItemQuery, IEnumerable<ScheduleDto>>
{
    private readonly IScheduleRepository _repository;
    private readonly IMapper _mapper;

    public GetSchedulesByItemQueryHandler(IScheduleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleDto>> Handle(GetSchedulesByItemQuery request, CancellationToken cancellationToken)
    {
        var schedules = await _repository.GetByItemAsync(request.ItemId);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }
}

public class GetSchedulesByDateRangeQueryHandler : IRequestHandler<GetSchedulesByDateRangeQuery, IEnumerable<ScheduleDto>>
{
    private readonly IScheduleRepository _repository;
    private readonly IMapper _mapper;

    public GetSchedulesByDateRangeQueryHandler(IScheduleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleDto>> Handle(GetSchedulesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var schedules = await _repository.GetByDateRangeAsync(request.FromDate, request.ToDate);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }
}

public class GetScheduleDetailsQueryHandler : IRequestHandler<GetScheduleDetailsQuery, IEnumerable<ScheduleDetailDto>>
{
    private readonly IScheduleRepository _repository;
    private readonly IMapper _mapper;

    public GetScheduleDetailsQueryHandler(IScheduleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleDetailDto>> Handle(GetScheduleDetailsQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        if (schedule == null) return Enumerable.Empty<ScheduleDetailDto>();

        return _mapper.Map<IEnumerable<ScheduleDetailDto>>(schedule.ScheduleDetails);
    }
}

public class GetAvailableCapacityQueryHandler : IRequestHandler<GetAvailableCapacityQuery, decimal>
{
    private readonly IScheduleRepository _repository;

    public GetAvailableCapacityQueryHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(GetAvailableCapacityQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        if (schedule == null) return 0;

        return schedule.GetRemainingCapacity();
    }
}
