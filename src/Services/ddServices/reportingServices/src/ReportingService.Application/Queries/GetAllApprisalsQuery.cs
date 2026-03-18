using MediatR;

namespace ReportingService.Application.Queries;

public class GetAllApprisalsQuery : IRequest<IEnumerable<DTOs.AppraisalDto>>
{
    public class Handler : IRequestHandler<GetAllApprisalsQuery, IEnumerable<DTOs.AppraisalDto>>
    {
        private readonly ReportingService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(ReportingService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.AppraisalDto>> Handle(GetAllApprisalsQuery request, CancellationToken cancellationToken)
        {
            var appraisals = await _unitOfWork.Appraisals.GetAllAsync(cancellationToken);
            return appraisals.Select(MapToDto).ToList();
        }

        private DTOs.AppraisalDto MapToDto(ReportingService.Domain.Entities.Appraisal appraisal)
        {
            return new DTOs.AppraisalDto
            {
                Id = appraisal.Id,
                RequestNumber = appraisal.RequestNumber,
                UserName = appraisal.UserName,
                UserId = appraisal.UserId,
                StatusDescription = appraisal.StatusDescription,
                FinancialPeriod = appraisal.FinancialPeriod,
                UnitCode = appraisal.UnitCode,
                GradeCode = appraisal.GradeCode,
                AcademicYear = appraisal.AcademicYear,
                DDType = appraisal.DDType,
                IsCompleted = appraisal.IsCompleted,
                CreatedAt = appraisal.CreatedAt,
                UpdatedAt = appraisal.UpdatedAt
            };
        }
    }
}
