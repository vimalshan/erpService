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
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var newId = await mainRepository.GetNextIdAsync(cancellationToken);

            var scholarship = ScholarshipMain.Create(
                newId, request.EmployeeSysId, request.GradeId, request.DependentId,
                request.ChildName, request.LastSchool, request.LastYearOfSchool, request.LastExam,
                request.CgpaFlag, request.MarksPercentage, request.MarksGpa, request.MarksFile,
                request.CourseName, request.CourseJoinYear, request.CourseJoinMonth, request.CourseDuration,
                request.AdmissionReceiptFile, request.PaymentMode, request.ChildAccountNumber,
                request.ChildBankIfsc, request.ChildBankMicr, request.Source,
                request.DisbursementAmount, request.DisbursementFrequency,
                request.CreatedBy, request.IsOffline, request.OfflineYear);

            await mainRepository.AddAsync(scholarship, cancellationToken);

            var detailId = await detailRepository.GetNextIdAsync(cancellationToken);
            var detail = ScholarshipDetail.Create(detailId, newId, request.CourseJoinYear, request.MarksFile, request.CreatedBy);
            await detailRepository.AddAsync(detail, cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Created scholarship {ScholarshipId} for employee {EmployeeId}", newId, request.EmployeeSysId);
            return newId;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to create scholarship for employee {EmployeeId}", request.EmployeeSysId);
            throw;
        }
    }
}
