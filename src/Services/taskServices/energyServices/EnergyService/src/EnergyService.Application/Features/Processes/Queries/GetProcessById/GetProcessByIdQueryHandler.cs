using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Processes.Queries.GetProcessById;

public class GetProcessByIdQueryHandler : IRequestHandler<GetProcessByIdQuery, EcProcessDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetProcessByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcProcessDto?> Handle(GetProcessByIdQuery request, CancellationToken ct)
    {
        var entity = await _uow.Processes.GetByIdAsync(request.Id, ct);
        return entity is null ? null : _mapper.Map<EcProcessDto>(entity);
    }
}
