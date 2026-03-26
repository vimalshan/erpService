using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.CreateLovTypeMaster;

public class CreateLovTypeMasterCommandHandler : IRequestHandler<CreateLovTypeMasterCommand, LovTypeMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLovTypeMasterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LovTypeMasterDto> Handle(CreateLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = new LovTypeMaster(request.LovTypeCode, request.LovTypeName);

        await _unitOfWork.LovTypeMasters.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LovTypeMasterDto>(entity);
    }
}
