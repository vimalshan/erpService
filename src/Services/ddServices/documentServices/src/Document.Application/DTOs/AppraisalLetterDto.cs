namespace Document.Application.DTOs;

public record AppraisalLetterDto(
    decimal SerialNo,
    decimal? BandCode,
    string? LetterType,
    DateTime? FromDate,
    DateTime? EndDate,
    string? Paragraph1,
    string? Paragraph2,
    string? Paragraph3,
    string? Paragraph4,
    string? Paragraph5,
    DateTime? EffectiveDate,
    DateTime? PrintDate);

public record CreateAppraisalLetterRequest(
    decimal SerialNo,
    string? LetterType,
    DateTime? FromDate,
    DateTime? EndDate,
    string? Paragraph1,
    string? Paragraph2,
    DateTime? EffectiveDate);
