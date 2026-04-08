using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Queries.GetAccumulations;

public class GetAccumulationsQueryHandler : IRequestHandler<GetAccumulationsQuery, IEnumerable<PFAccumulationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccumulationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PFAccumulationDto>> Handle(GetAccumulationsQuery request, CancellationToken cancellationToken)
    {
        var accumulations = await _unitOfWork.Accumulations.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PFAccumulationDto>>(accumulations);
    }
}
