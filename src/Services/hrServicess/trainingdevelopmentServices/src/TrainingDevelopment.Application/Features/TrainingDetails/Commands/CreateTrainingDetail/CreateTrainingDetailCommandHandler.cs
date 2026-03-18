using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Entities;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.CreateTrainingDetail;

public class CreateTrainingDetailCommandHandler : IRequestHandler<CreateTrainingDetailCommand, TrainingDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTrainingDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDetailDto> Handle(CreateTrainingDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = TrainingDetail.Create(
            request.Id,
            request.FinancialYear,
            request.EmployeeSysId,
            request.TrainingNeed,
            request.GapArea,
            request.Mode,
            request.ProgramId,
            request.ProgramDescription,
            request.PlannedFrom,
            request.PlannedTo,
            request.LastModifiedBy);

        await _unitOfWork.TrainingDetails.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TrainingDetailDto>(entity);
    }
}
