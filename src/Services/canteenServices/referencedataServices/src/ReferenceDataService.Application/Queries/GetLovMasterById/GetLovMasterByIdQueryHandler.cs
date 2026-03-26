using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Queries.GetLovMasterById;

public class GetLovMasterByIdQueryHandler : IRequestHandler<GetLovMasterByIdQuery, LovMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLovMasterByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovMasterDto?> Handle(GetLovMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovMasters.GetByIdAsync(request.LovId, cancellationToken);
        return entity == null ? null : _mapper.Map<LovMasterDto>(entity);
    }
}
