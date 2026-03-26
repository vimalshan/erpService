using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.CreateLovMaster;

public class CreateLovMasterCommandHandler : IRequestHandler<CreateLovMasterCommand, LovMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLovMasterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovMasterDto> Handle(CreateLovMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = new LovMaster(request.LovId, request.LovType, request.LovName);

        await _unitOfWork.LovMasters.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LovMasterDto>(entity);
    }
}
