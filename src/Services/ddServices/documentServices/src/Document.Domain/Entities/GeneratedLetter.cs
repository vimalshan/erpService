using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_GENERATELETTER — a generated letter record for an employee.
/// </summary>
public class GeneratedLetter : BaseEntity
{
    public decimal? CreatedByPin { get; private set; }
    public decimal? EmployeePin { get; private set; }
    public string? EmployeeName { get; private set; }
    public string? SignatoryName { get; private set; }
    public string? SignatoryDesignation { get; private set; }
    public string? EmployeeRandomNumber { get; private set; }
    public string? EmployeeUnitCode { get; private set; }
    public DateTime? PrintDate { get; private set; }
    public decimal? AppraisalLumpsum { get; private set; }
    public decimal? AppraisalBasicPay { get; private set; }
    public decimal? AppraisalFlexiPay { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public string? LetterType { get; private set; }
    public string? FinalRating { get; private set; }
    public decimal? AppraisalIncrement { get; private set; }
    public decimal? PromotionLevel { get; private set; }
    public string? AppraisalDesignation { get; private set; }
    public string? AppraisalBand { get; private set; }
    public string? SignatoryName2 { get; private set; }
    public string? SignatoryDesignation2 { get; private set; }
    public decimal? IncrementTemplateId { get; private set; }
    public decimal? RatingTemplateId { get; private set; }

    private GeneratedLetter() { }

    public static GeneratedLetter Create(
        decimal? createdByPin,
        decimal? employeePin,
        string? employeeName,
        string? letterType,
        DateTime? effectiveDate)
    {
        var letter = new GeneratedLetter
        {
            CreatedByPin = createdByPin,
            EmployeePin = employeePin,
            EmployeeName = employeeName,
            LetterType = letterType,
            EffectiveDate = effectiveDate,
            PrintDate = DateTime.UtcNow
        };
        letter.AddDomainEvent(new Events.LetterGeneratedEvent(letter));
        return letter;
    }
}
