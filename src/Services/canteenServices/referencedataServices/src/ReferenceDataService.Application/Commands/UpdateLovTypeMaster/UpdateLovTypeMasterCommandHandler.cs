using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.UpdateLovTypeMaster;

public class UpdateLovTypeMasterCommandHandler : IRequestHandler<UpdateLovTypeMasterCommand, LovTypeMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLovTypeMasterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovTypeMasterDto> Handle(UpdateLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovTypeMasters.GetByCodeAsync(request.LovTypeCode, cancellationToken)
            ?? throw new KeyNotFoundException($"LovTypeMaster with code '{request.LovTypeCode}' not found.");

        entity.Update(request.LovTypeName);
        _unitOfWork.LovTypeMasters.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LovTypeMasterDto>(entity);
    }
}
