using FaqServices.Application.Common.DTOs;

namespace FaqServices.API.GraphQL.Types;

public class FaqGradeType
{
    public string PK { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int QuestionCount { get; set; }
    
    [GraphQLIgnore]
    public IEnumerable<FaqQuestionType>? Questions { get; set; }
}

public class FaqQuestionType
{
    public string PK { get; set; } = string.Empty;
    public string GradeId { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionTextAr { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? ImageBlobUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    [GraphQLIgnore]
    public List<FaqAnswerType> Answers { get; set; } = new();
}

public class FaqAnswerType
{
    public string PK { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
    public string? AnswerTextAr { get; set; }
    public bool IsCorrect { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? ImageBlobUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
