namespace OrderScheduleService.Application.QueryHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Queries;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Domain.Interfaces;

public class GetShiftByIdQueryHandler : IRequestHandler<GetShiftByIdQuery, ShiftDto?>
{
    private readonly IShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetShiftByIdQueryHandler(IShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ShiftDto?> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        var shift = await _repository.GetByIdAsync(request.ShiftCode, request.CompanyUnitId);
        return _mapper.Map<ShiftDto>(shift);
    }
}

public class GetShiftsByCompanyQueryHandler : IRequestHandler<GetShiftsByCompanyQuery, IEnumerable<ShiftDto>>
{
    private readonly IShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetShiftsByCompanyQueryHandler(IShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShiftDto>> Handle(GetShiftsByCompanyQuery request, CancellationToken cancellationToken)
    {
        var shifts = await _repository.GetByCompanyAsync(request.CompanyUnitId);
        return _mapper.Map<IEnumerable<ShiftDto>>(shifts);
    }
}

public class GetAllShiftsQueryHandler : IRequestHandler<GetAllShiftsQuery, IEnumerable<ShiftDto>>
{
    private readonly IShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetAllShiftsQueryHandler(IShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShiftDto>> Handle(GetAllShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ShiftDto>>(shifts);
    }
}
