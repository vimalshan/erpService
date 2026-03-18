using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Commands.Surveys;

public record CreateSurveyCommand(CreateSurveyDto Dto, long UserId) : IRequest<SurveyDto>;

public record CreateSurveyQuestionCommand(
    long SurveyId,
    long ControlId,
    long UnitId,
    long OwnerId,
    long ApproverId,
    DateTime DueDate,
    long UserId) : IRequest<SurveyQuestionDto>;

public record SubmitSurveyFeedbackCommand(
    long SurveyQuestionId,
    long EmployeeSysId,
    char Status,
    char Type,
    char RemedialFlag,
    string? Remarks,
    char EvidenceFlag) : IRequest<SurveyFeedbackDto>;

public record ApproveSurveyFeedbackCommand(
    long FeedbackId,
    long ApproverId,
    char ApprovalFlag,
    string Remarks) : IRequest<SurveyFeedbackDto>;
