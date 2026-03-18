namespace FaqServices.Application.Common.DTOs;

public class FaqGradeDto
{
    public string PK { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int QuestionCount { get; set; }
}

public class FaqQuestionDto
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
    public List<FaqAnswerDto> Answers { get; set; } = new();
}

public class FaqAnswerDto
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
