using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Queries.GetAllLovTypeMasters;

public class GetAllLovTypeMastersQueryHandler : IRequestHandler<GetAllLovTypeMastersQuery, IEnumerable<LovTypeMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLovTypeMastersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LovTypeMasterDto>> Handle(GetAllLovTypeMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.LovTypeMasters.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LovTypeMasterDto>>(entities);
    }
}
