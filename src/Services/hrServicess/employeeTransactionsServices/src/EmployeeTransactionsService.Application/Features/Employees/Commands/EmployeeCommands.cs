using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Domain.Entities;
using EmployeeTransactionsService.Domain.Interfaces;
using EmployeeTransactionsService.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace EmployeeTransactionsService.Application.Features.Employees.Commands;

public sealed record CreateEmployeeCommand(
    decimal EmpPinNo,
    DateTime AppDate,
    string AppUnit,
    decimal AppGrade,
    decimal AppPosition,
    string AppPositionDesc,
    string FirstName,
    string? MiddleName,
    string? LastName,
    string Gender,
    DateTime Dob,
    string OfferStatus,
    string? OfficialEmail,
    string? PersonalEmail,
    string? MobileNo,
    string LeadRole,
    DateTime? ProbationDueDate,
    decimal AppUnitId,
    decimal CreatedBy) : IRequest<decimal>;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmpPinNo).GreaterThan(0);
        RuleFor(x => x.AppUnit).NotEmpty().MaximumLength(3);
        RuleFor(x => x.AppGrade).GreaterThan(0);
        RuleFor(x => x.AppPosition).GreaterThan(0);
        RuleFor(x => x.AppPositionDesc).NotEmpty().MaximumLength(150);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.Gender).NotEmpty().MaximumLength(1);
        RuleFor(x => x.OfferStatus).NotEmpty().MaximumLength(1);
        RuleFor(x => x.LeadRole).NotEmpty().MaximumLength(3);
    }
}

public sealed class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeProbationRepository probationRepository,
    IEmployeeGradeRepository gradeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeCommand, decimal>
{
    public async Task<decimal> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeId = await employeeRepository.GetNextIdAsync(cancellationToken);
        var employee = EmployeeMain.Create(
            employeeId,
            request.EmpPinNo,
            request.AppDate,
            request.AppUnit,
            request.AppGrade,
            request.AppPosition,
            request.AppPositionDesc,
            EmployeeName.Create(request.FirstName, request.MiddleName, request.LastName),
            request.Gender,
            request.Dob,
            request.OfferStatus,
            EmailAddress.CreateOptional(request.OfficialEmail),
            EmailAddress.CreateOptional(request.PersonalEmail),
            request.MobileNo,
            request.LeadRole,
            request.ProbationDueDate,
            request.AppUnitId,
            request.CreatedBy);

        await employeeRepository.AddAsync(employee, cancellationToken);

        var gradeTranId = await gradeRepository.GetNextTransactionIdAsync(cancellationToken);
        var grade = EmployeeGrade.Create(employeeId, gradeTranId, request.AppGrade, request.AppDate, request.CreatedBy, request.ProbationDueDate.HasValue ? "Y" : "N");
        await gradeRepository.AddAsync(grade, cancellationToken);

        if (request.ProbationDueDate.HasValue)
        {
            var probationId = await probationRepository.GetNextIdAsync(cancellationToken);
            var probation = EmployeeProbation.CreateInitial(probationId, employeeId, request.ProbationDueDate.Value);
            await probationRepository.AddAsync(probation, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return employeeId;
    }
}

public sealed record RegisterGradeChangeCommand(
    decimal EmployeeId,
    decimal NewGradeId,
    DateTime EffectiveDate,
    string Status,
    decimal CreatedBy) : IRequest<decimal>;

public sealed class RegisterGradeChangeCommandValidator : AbstractValidator<RegisterGradeChangeCommand>
{
    public RegisterGradeChangeCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
        RuleFor(x => x.NewGradeId).GreaterThan(0);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(1);
    }
}

public sealed class RegisterGradeChangeCommandHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeGradeRepository gradeRepository,
    IEmployeeGradeChangeRepository gradeChangeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterGradeChangeCommand, decimal>
{
    public async Task<decimal> Handle(RegisterGradeChangeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {request.EmployeeId} not found.");

        var currentGrade = await gradeRepository.GetCurrentByEmployeeAsync(request.EmployeeId, cancellationToken);
        var oldGradeId = currentGrade?.GradeId ?? employee.EmpAppGrade;

        if (currentGrade is not null)
            currentGrade.Close(request.EffectiveDate, request.CreatedBy);

        var changeId = await gradeChangeRepository.GetNextIdAsync(cancellationToken);
        var gradeChange = EmployeeGradeChange.Create(changeId, request.EmployeeId, oldGradeId, request.NewGradeId, request.EffectiveDate, request.Status, request.CreatedBy);
        await gradeChangeRepository.AddAsync(gradeChange, cancellationToken);

        var nextTranId = await gradeRepository.GetNextTransactionIdAsync(cancellationToken);
        var newGrade = EmployeeGrade.Create(request.EmployeeId, nextTranId, request.NewGradeId, request.EffectiveDate, request.CreatedBy, "N");
        await gradeRepository.AddAsync(newGrade, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return changeId;
    }
}

public sealed record ReviewProbationCommand(
    decimal ProbationId,
    string FinalStatus,
    DateTime? ConfirmationDate,
    DateTime? NextReviewDate,
    decimal ReviewedBy) : IRequest<bool>;

public sealed class ReviewProbationCommandValidator : AbstractValidator<ReviewProbationCommand>
{
    public ReviewProbationCommandValidator()
    {
        RuleFor(x => x.ProbationId).GreaterThan(0);
        RuleFor(x => x.FinalStatus).Must(status => status is "A" or "B" or "C");
    }
}

public sealed class ReviewProbationCommandHandler(
    IEmployeeProbationRepository probationRepository,
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReviewProbationCommand, bool>
{
    public async Task<bool> Handle(ReviewProbationCommand request, CancellationToken cancellationToken)
    {
        var probation = await probationRepository.GetByIdAsync(request.ProbationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Probation record {request.ProbationId} not found.");

        var employee = await employeeRepository.GetByIdAsync(probation.ProbEmpSysId, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {probation.ProbEmpSysId} not found.");

        probation.Review(request.FinalStatus, request.ConfirmationDate, request.NextReviewDate);
        employee.ApplyProbationReview(request.FinalStatus, request.ConfirmationDate, request.NextReviewDate, request.ReviewedBy);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public sealed record RegisterStationeryImageCommand(
    string ItemReference,
    string FileName,
    string ContentType,
    byte[] Content,
    decimal UploadedBy) : IRequest<StationeryImageDto>;

public sealed class RegisterStationeryImageCommandValidator : AbstractValidator<RegisterStationeryImageCommand>
{
    public RegisterStationeryImageCommandValidator()
    {
        RuleFor(x => x.ItemReference).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }
}

public sealed class RegisterStationeryImageCommandHandler(
    IBlobStorageService blobStorageService,
    IStationeryItemImageRepository stationeryItemImageRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterStationeryImageCommand, StationeryImageDto>
{
    public async Task<StationeryImageDto> Handle(RegisterStationeryImageCommand request, CancellationToken cancellationToken)
    {
        var blobName = $"stationery/{request.ItemReference}/{Guid.NewGuid():N}-{request.FileName}";
        await blobStorageService.UploadAsync(blobName, request.Content, request.ContentType, cancellationToken);

        var image = StationeryItemImage.Create(request.ItemReference, blobName, request.ContentType, request.UploadedBy);
        await stationeryItemImageRepository.AddAsync(image, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return image.ToDto();
    }
}