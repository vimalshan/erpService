using MediatR;
using Microsoft.Extensions.Logging;
using ScholarshipService.Application.Common;
using ScholarshipService.Domain.Entities;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Commands.CreateScholarship;

public class CreateScholarshipCommandHandler(
    IScholarshipMainRepository mainRepository,
    IScholarshipDetailRepository detailRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateScholarshipCommandHandler> logger)
    : IRequestHandler<CreateScholarshipCommand, int>
{
    public async Task<int> Handle(CreateScholarshipCommand request, CancellationToken cancellationToken)
    {
        var newId = await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var id = await mainRepository.GetNextIdAsync(ct);

            var scholarship = ScholarshipMain.Create(
                id, request.EmployeeSysId, request.GradeId, request.DependentId,
                request.ChildName, request.LastSchool, request.LastYearOfSchool, request.LastExam,
                request.CgpaFlag, request.MarksPercentage, request.MarksGpa, request.MarksFile,
                request.CourseName, request.CourseJoinYear, request.CourseJoinMonth, request.CourseDuration,
                request.AdmissionReceiptFile, request.PaymentMode, request.ChildAccountNumber,
                request.ChildBankIfsc, request.ChildBankMicr, request.Source,
                request.DisbursementAmount, request.DisbursementFrequency,
                request.CreatedBy, request.IsOffline, request.OfflineYear);

            await mainRepository.AddAsync(scholarship, ct);

            var detailId = await detailRepository.GetNextIdAsync(ct);
            var detail = ScholarshipDetail.Create(detailId, id, request.CourseJoinYear, request.MarksFile, request.CreatedBy);
            await detailRepository.AddAsync(detail, ct);

            return id;
        }, cancellationToken);

        logger.LogInformation("Created scholarship {ScholarshipId} for employee {EmployeeId}", newId, request.EmployeeSysId);
        return newId;
    }
}
