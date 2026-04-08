using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Queries.GetAccumulation;

public class GetAccumulationQueryHandler : IRequestHandler<GetAccumulationQuery, PFAccumulationDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccumulationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PFAccumulationDto?> Handle(GetAccumulationQuery request, CancellationToken cancellationToken)
    {
        var accumulation = await _unitOfWork.Accumulations.GetByEmpSysIdAsync(request.EmpSysId, cancellationToken);
        return accumulation is null ? null : _mapper.Map<PFAccumulationDto>(accumulation);
    }
}
