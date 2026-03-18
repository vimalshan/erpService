using MediatR;

namespace ReportingService.Application.Commands;

public class CompleteAppraisalCommand : IRequest<bool>
{
    public long AppraisalId { get; set; }

    public class Handler : IRequestHandler<CompleteAppraisalCommand, bool>
    {
        private readonly ReportingService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(ReportingService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CompleteAppraisalCommand request, CancellationToken cancellationToken)
        {
            var appraisal = await _unitOfWork.Appraisals.GetByIdAsync(request.AppraisalId, cancellationToken);
            if (appraisal == null)
                return false;

            appraisal.MarkAsCompleted();
            await _unitOfWork.Appraisals.UpdateAsync(appraisal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
