using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.UpdateTrainingDetail;

public class UpdateTrainingDetailCommandHandler : IRequestHandler<UpdateTrainingDetailCommand, TrainingDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTrainingDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDetailDto> Handle(UpdateTrainingDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TrainingDetails.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TrainingDetail), request.Id);

        entity.Update(
            request.TrainingNeed,
            request.GapArea,
            request.Mode,
            request.ProgramId,
            request.ProgramDescription,
            request.PlannedFrom,
            request.PlannedTo,
            request.LastModifiedBy);

        _unitOfWork.TrainingDetails.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TrainingDetailDto>(entity);
    }
}
