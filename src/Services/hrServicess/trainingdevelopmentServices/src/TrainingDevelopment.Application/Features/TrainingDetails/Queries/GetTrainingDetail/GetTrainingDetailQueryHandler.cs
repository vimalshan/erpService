using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetail;

public class GetTrainingDetailQueryHandler : IRequestHandler<GetTrainingDetailQuery, TrainingDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTrainingDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDetailDto> Handle(GetTrainingDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TrainingDetails.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TrainingDetail), request.Id);

        return _mapper.Map<TrainingDetailDto>(entity);
    }
}
