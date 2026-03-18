namespace CheckupManagementService.Application.Commands;

using MediatR;
using FluentValidation;
using Shared.Infrastructure.Utilities;

/// <summary>
/// Command to create a new checkup
/// </summary>
public class CreateCheckupCommand : IRequest<CreateCheckupResponse>
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CheckupType { get; set; } = string.Empty;
    public DateTime CheckupDate { get; set; }
    public string? DoctorCode { get; set; }
    public List<string> TestIds { get; set; } = new();

    public class Validator : AbstractValidator<CreateCheckupCommand>
    {
        public Validator()
        {
            RuleFor(x => x.EmployeeNumber)
                .NotEmpty().WithMessage("Employee number is required")
                .Must(x => ValidationUtilities.IsValidEmployeeNumber(x))
                .WithMessage("Invalid employee number format");

            RuleFor(x => x.CheckupType)
                .NotEmpty().WithMessage("Checkup type is required")
                .MaximumLength(50);

            RuleFor(x => x.CheckupDate)
                .NotEmpty()
                .Must(x => !ValidationUtilities.IsValidFutureDate(x))
                .WithMessage("Checkup date cannot be in the future");
        }
    }
}

/// <summary>
/// Command to update checkup status
/// </summary>
public class UpdateCheckupStatusCommand : IRequest<UpdateCheckupResponse>
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? DoctorRemarks { get; set; }
    public string? ApprovedBy { get; set; }

    public class Validator : AbstractValidator<UpdateCheckupStatusCommand>
    {
        public Validator()
        {
            RuleFor(x => x.CheckupMasterId)
                .NotEmpty().WithMessage("Checkup ID is required");

            RuleFor(x => x.Status)
                .MaximumLength(50);
        }
    }
}

/// <summary>
/// Command to record health examination
/// </summary>
public class RecordHealthExaminationCommand : IRequest<RecordHealthExaminationResponse>
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public string? BloodGroup { get; set; }
    public string? EyeVision { get; set; }
    public List<HealthTestResultInput> TestResults { get; set; } = new();

    public class Validator : AbstractValidator<RecordHealthExaminationCommand>
    {
        public Validator()
        {
            RuleFor(x => x.CheckupMasterId).NotEmpty();
            RuleFor(x => x.EmployeeNumber).NotEmpty();
            RuleFor(x => x.Height).GreaterThan(0).When(x => x.Height.HasValue);
            RuleFor(x => x.Weight).GreaterThan(0).When(x => x.Weight.HasValue);
            RuleFor(x => x.HeartRate).GreaterThan(0).When(x => x.HeartRate.HasValue);
        }
    }
}

public class HealthTestResultInput
{
    public string TestName { get; set; } = string.Empty;
    public string? TestValue { get; set; }
    public string? Result { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Command to create test master
/// </summary>
public class CreateTestMasterCommand : IRequest<CreateTestMasterResponse>
{
    public string TestName { get; set; } = string.Empty;
    public string? TestCategory { get; set; }
    public string? NormalRange { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }

    public class Validator : AbstractValidator<CreateTestMasterCommand>
    {
        public Validator()
        {
            RuleFor(x => x.TestName)
                .NotEmpty().WithMessage("Test name is required")
                .MaximumLength(100);

            RuleFor(x => x.Cost)
                .GreaterThan(0).When(x => x.Cost.HasValue)
                .WithMessage("Cost must be greater than zero");
        }
    }
}

/// <summary>
/// Command to update test master
/// </summary>
public class UpdateTestMasterCommand : IRequest<UpdateTestMasterResponse>
{
    public string TestId { get; set; } = string.Empty;
    public string? TestName { get; set; }
    public string? TestCategory { get; set; }
    public string? NormalRange { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
    public bool? IsActive { get; set; }

    public class Validator : AbstractValidator<UpdateTestMasterCommand>
    {
        public Validator()
        {
            RuleFor(x => x.TestId).NotEmpty().WithMessage("Test ID is required");
            RuleFor(x => x.TestName).MaximumLength(100).When(x => x.TestName != null);
            RuleFor(x => x.Cost)
                .GreaterThan(0).When(x => x.Cost.HasValue)
                .WithMessage("Cost must be greater than zero");
        }
    }
}

/// <summary>
/// Command to record checkup other details
/// </summary>
public class RecordCheckupOthersCommand : IRequest<RecordCheckupOthersResponse>
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string? MedicineAllergy { get; set; }
    public string? FamilyHistory { get; set; }
    public string? PastSurgery { get; set; }
    public string? CurrentMedicines { get; set; }
    public string? LifestyleHabits { get; set; }
    public string? OtherComments { get; set; }

    public class Validator : AbstractValidator<RecordCheckupOthersCommand>
    {
        public Validator()
        {
            RuleFor(x => x.CheckupMasterId).NotEmpty();
        }
    }
}

/// <summary>
/// Command to issue health check card
/// </summary>
public class IssueHealthCheckCardCommand : IRequest<IssueHealthCheckCardResponse>
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public string? IssuedBy { get; set; }

    public class Validator : AbstractValidator<IssueHealthCheckCardCommand>
    {
        public Validator()
        {
            RuleFor(x => x.CheckupMasterId).NotEmpty();
            RuleFor(x => x.EmployeeNumber).NotEmpty();
        }
    }
}

// Response types
public class CreateCheckupResponse
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CheckupType { get; set; } = string.Empty;
    public DateTime CheckupDate { get; set; }
    public string Status { get; set; } = "Pending";
}

public class UpdateCheckupResponse
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string? Status { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

public class RecordHealthExaminationResponse
{
    public string HealthId { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public decimal? BMI { get; set; }
    public string? Message { get; set; }
}

public class CreateTestMasterResponse
{
    public string TestId { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public string? TestCategory { get; set; }
}

public class UpdateTestMasterResponse
{
    public string TestId { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
    public bool IsActive { get; set; }
}

public class RecordCheckupOthersResponse
{
    public string CheckupOthersId { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
}

public class IssueHealthCheckCardResponse
{
    public string CardNumber { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string CardStatus { get; set; } = "Valid";
}

/// <summary>
/// Custom exception for validation errors
/// </summary>
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Failures { get; }

    public ValidationException(Dictionary<string, string[]> failures = null!)
    {
        Failures = failures ?? new Dictionary<string, string[]>();
    }
}
