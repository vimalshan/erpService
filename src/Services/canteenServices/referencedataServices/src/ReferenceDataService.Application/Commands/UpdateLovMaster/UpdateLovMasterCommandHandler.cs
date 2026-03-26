using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.UpdateLovMaster;

public class UpdateLovMasterCommandHandler : IRequestHandler<UpdateLovMasterCommand, LovMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLovMasterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovMasterDto> Handle(UpdateLovMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovMasters.GetByIdAsync(request.LovId, cancellationToken)
            ?? throw new KeyNotFoundException($"LovMaster with Id '{request.LovId}' not found.");

        entity.Update(request.LovType, request.LovName);
        _unitOfWork.LovMasters.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LovMasterDto>(entity);
    }
}
