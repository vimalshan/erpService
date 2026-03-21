using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Readings.Queries.GetReadingById;

public class GetReadingByIdQueryHandler : IRequestHandler<GetReadingByIdQuery, EcReadingDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetReadingByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcReadingDto?> Handle(GetReadingByIdQuery request, CancellationToken ct)
    {
        var entity = await _uow.Readings.GetByIdAsync(request.Id, ct);
        return entity is null ? null : _mapper.Map<EcReadingDto>(entity);
    }
}
