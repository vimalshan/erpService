using MediatR;

namespace ReportingService.Application.Commands;

public class CreateAppraisalCommand : IRequest<long>
{
    public long RequestNumber { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public string? StatusDescription { get; set; }
    public string? FinancialPeriod { get; set; }
    public string? UnitCode { get; set; }
    public string? GradeCode { get; set; }
    public string? AcademicYear { get; set; }
    public string? DDType { get; set; }

    public class Handler : IRequestHandler<CreateAppraisalCommand, long>
    {
        private readonly ReportingService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(ReportingService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateAppraisalCommand request, CancellationToken cancellationToken)
        {
            var appraisal = new ReportingService.Domain.Entities.Appraisal(
                request.RequestNumber,
                request.UserName,
                request.UserId)
            {
                StatusDescription = request.StatusDescription,
                FinancialPeriod = request.FinancialPeriod,
                UnitCode = request.UnitCode,
                GradeCode = request.GradeCode,
                AcademicYear = request.AcademicYear,
                DDType = request.DDType
            };

            await _unitOfWork.Appraisals.AddAsync(appraisal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return appraisal.Id;
        }
    }
}
