using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Queries.GetLovTypeMasterByCode;

public class GetLovTypeMasterByCodeQueryHandler : IRequestHandler<GetLovTypeMasterByCodeQuery, LovTypeMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLovTypeMasterByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovTypeMasterDto?> Handle(GetLovTypeMasterByCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovTypeMasters.GetByCodeAsync(request.LovTypeCode, cancellationToken);
        return entity == null ? null : _mapper.Map<LovTypeMasterDto>(entity);
    }
}
