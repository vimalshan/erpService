using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetailList;

public class GetTrainingDetailListQueryHandler : IRequestHandler<GetTrainingDetailListQuery, IEnumerable<TrainingDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTrainingDetailListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrainingDetailDto>> Handle(GetTrainingDetailListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.TrainingDetail> entities;

        if (request.EmployeeSysId.HasValue)
            entities = await _unitOfWork.TrainingDetails.GetByEmployeeAsync(request.EmployeeSysId.Value, cancellationToken);
        else if (request.FinancialYear.HasValue)
            entities = await _unitOfWork.TrainingDetails.GetByFinancialYearAsync(request.FinancialYear.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.Status))
            entities = await _unitOfWork.TrainingDetails.GetByStatusAsync(request.Status, cancellationToken);
        else
            entities = await _unitOfWork.TrainingDetails.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<TrainingDetailDto>>(entities);
    }
}
