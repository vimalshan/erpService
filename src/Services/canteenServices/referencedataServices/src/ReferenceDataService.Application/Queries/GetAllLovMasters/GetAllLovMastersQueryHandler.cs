using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Queries.GetAllLovMasters;

public class GetAllLovMastersQueryHandler : IRequestHandler<GetAllLovMastersQuery, IEnumerable<LovMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLovMastersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.LovMasters.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LovMasterDto>>(entities);
    }
}
