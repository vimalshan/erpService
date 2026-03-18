using Todos.Domain.Abstractions;

namespace Todos.Domain.Entities;

/// <summary>
/// Represents a development category detail record
/// </summary>
public class DevelopmentCategoryDetail : Entity
{
    /// <summary>
    /// Gets the DD request number
    /// </summary>
    public decimal RequestNumber { get; private set; }

    /// <summary>
    /// Gets the question number
    /// </summary>
    public decimal QuestionNumber { get; private set; }

    /// <summary>
    /// Gets the answer serial number
    /// </summary>
    public decimal AnswerSerial { get; private set; }

    /// <summary>
    /// Gets the employee ID
    /// </summary>
    public string? EmployeeId { get; private set; }

    /// <summary>
    /// Gets the employee number
    /// </summary>
    public decimal EmployeeNumber { get; private set; }

    /// <summary>
    /// Gets the areas for development
    /// </summary>
    public string? DevelopmentArea { get; private set; }

    /// <summary>
    /// Gets why the development is needed
    /// </summary>
    public string? Need { get; private set; }

    /// <summary>
    /// Gets the entry date
    /// </summary>
    public DateTime? EntryDate { get; private set; }

    /// <summary>
    /// Initializes a new instance of the DevelopmentCategoryDetail class
    /// </summary>
    protected DevelopmentCategoryDetail() { }

    /// <summary>
    /// Creates a new development category detail
    /// </summary>
    public static DevelopmentCategoryDetail Create(
        decimal requestNumber,
        decimal questionNumber,
        decimal answerSerial,
        string? employeeId,
        decimal employeeNumber,
        string? developmentArea,
        string? need)
    {
        return new DevelopmentCategoryDetail
        {
            RequestNumber = requestNumber,
            QuestionNumber = questionNumber,
            AnswerSerial = answerSerial,
            EmployeeId = employeeId,
            EmployeeNumber = employeeNumber,
            DevelopmentArea = developmentArea,
            Need = need,
            EntryDate = DateTime.UtcNow
        };
    }
}
