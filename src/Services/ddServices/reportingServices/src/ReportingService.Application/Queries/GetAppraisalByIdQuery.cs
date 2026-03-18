using MediatR;

namespace ReportingService.Application.Queries;

public class GetAppraisalByIdQuery : IRequest<DTOs.AppraisalDto?>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<GetAppraisalByIdQuery, DTOs.AppraisalDto?>
    {
        private readonly ReportingService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(ReportingService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DTOs.AppraisalDto?> Handle(GetAppraisalByIdQuery request, CancellationToken cancellationToken)
        {
            var appraisal = await _unitOfWork.Appraisals.GetByIdAsync(request.Id, cancellationToken);
            if (appraisal == null)
                return null;

            return MapToDto(appraisal);
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
                UpdatedAt = appraisal.UpdatedAt,
                Goals = appraisal.Goals.Select(g => new DTOs.AppraisalGoalDto
                {
                    Id = g.Id,
                    RequestNumber = g.RequestNumber,
                    Description = g.Description,
                    Weightage = g.Weightage,
                    Achievement = g.Achievement,
                    Category = g.Category
                }).ToList(),
                Performances = appraisal.Performances.Select(p => new DTOs.AppraiseePerformanceDto
                {
                    Id = p.Id,
                    RequestNumber = p.RequestNumber,
                    Description = p.Description,
                    MeanRating = p.MeanRating,
                    PerformanceRatingValue = p.PerformanceRatingValue,
                    PerformanceCategory = p.PerformanceCategory
                }).ToList()
            };
        }
    }
}
