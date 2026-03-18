using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Aggregates;
using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Events;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Commands.Surveys;

public class CreateSurveyCommandHandler(
    ISurveyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<CreateSurveyCommand, SurveyDto>
{
    public async Task<SurveyDto> Handle(CreateSurveyCommand request, CancellationToken ct)
    {
        var survey = mapper.Map<Survey>(request.Dto);
        survey.CreatedBy = request.UserId;
        survey.CreatedOn = DateTime.UtcNow;

        var aggregate = SurveyAggregate.Create(survey);
        var created = await repository.AddAsync(aggregate.Survey, ct);
        await unitOfWork.SaveChangesAsync(ct);

        foreach (var e in created.DomainEvents)
            await mediator.Publish(e, ct);
        created.ClearDomainEvents();

        return mapper.Map<SurveyDto>(created);
    }
}

public class CreateSurveyQuestionCommandHandler(
    ISurveyQuestionRepository questionRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateSurveyQuestionCommand, SurveyQuestionDto>
{
    public async Task<SurveyQuestionDto> Handle(CreateSurveyQuestionCommand request, CancellationToken ct)
    {
        var question = new SurveyQuestion
        {
            SurveyId = request.SurveyId,
            ControlId = request.ControlId,
            UnitId = request.UnitId,
            OwnerId = request.OwnerId,
            ApproverId = request.ApproverId,
            OriginalDueDate = request.DueDate,
            DueDate = request.DueDate,
            AssessmentFlag = 'P',
            ApprovalFlag = 'N',
            RemedialFlag = 'N',
            CreatedBy = request.UserId,
            CreatedOn = DateTime.UtcNow
        };

        var created = await questionRepository.AddAsync(question, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<SurveyQuestionDto>(created);
    }
}

public class SubmitSurveyFeedbackCommandHandler(
    ISurveyFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<SubmitSurveyFeedbackCommand, SurveyFeedbackDto>
{
    public async Task<SurveyFeedbackDto> Handle(SubmitSurveyFeedbackCommand request, CancellationToken ct)
    {
        var feedback = new SurveyFeedback
        {
            SurveyQuestionId = request.SurveyQuestionId,
            EmployeeSysId = request.EmployeeSysId,
            Status = request.Status,
            Type = request.Type,
            RemedialFlag = request.RemedialFlag,
            Remarks = request.Remarks,
            EnteredOn = DateTime.UtcNow,
            EvidenceFlag = request.EvidenceFlag,
            ApprovalFlag = 'P',
            ApproverRemarks = string.Empty
        };

        var created = await feedbackRepository.AddAsync(feedback, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await mediator.Publish(new SurveyFeedbackSubmittedEvent(created.FeedbackId, request.SurveyQuestionId, request.Status), ct);

        return mapper.Map<SurveyFeedbackDto>(created);
    }
}

public class ApproveSurveyFeedbackCommandHandler(
    ISurveyFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<ApproveSurveyFeedbackCommand, SurveyFeedbackDto>
{
    public async Task<SurveyFeedbackDto> Handle(ApproveSurveyFeedbackCommand request, CancellationToken ct)
    {
        var feedback = await feedbackRepository.GetByIdAsync(request.FeedbackId, ct)
            ?? throw new KeyNotFoundException($"Feedback {request.FeedbackId} not found.");

        feedback.ApprovalFlag = request.ApprovalFlag;
        feedback.ApproverRemarks = request.Remarks;
        feedback.ApprovedBy = request.ApproverId;
        feedback.ApprovalDate = DateTime.UtcNow;

        await feedbackRepository.UpdateAsync(feedback, ct);
        await unitOfWork.SaveChangesAsync(ct);

        if (request.ApprovalFlag == 'Y')
            await mediator.Publish(new SurveyFeedbackApprovedEvent(request.FeedbackId, request.ApproverId), ct);
        else
            await mediator.Publish(new SurveyFeedbackRejectedEvent(request.FeedbackId, request.ApproverId), ct);

        return mapper.Map<SurveyFeedbackDto>(feedback);
    }
}
