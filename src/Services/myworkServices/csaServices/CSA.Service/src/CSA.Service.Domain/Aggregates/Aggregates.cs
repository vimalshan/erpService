using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Events;

namespace CSA.Service.Domain.Aggregates;

public class ControlAggregate
{
    public Control Control { get; }

    public ControlAggregate(Control control)
    {
        Control = control;
    }

    public void AddEvidence(Evidence evidence)
    {
        Control.Evidences.Add(evidence);
        Control.AddDomainEvent(new EvidenceUploadedEvent(evidence.EvidenceId, Control.ControlId, evidence.Name));
    }

    public void AddUnitMapping(UnitMapDetail mapping)
    {
        Control.UnitMappings.Add(mapping);
        Control.AddDomainEvent(new UnitMappingCreatedEvent(mapping.MapId, Control.ControlId, mapping.UnitId));
    }

    public static ControlAggregate Create(Control control)
    {
        control.AddDomainEvent(new ControlCreatedEvent(control.ControlId, control.Title));
        return new ControlAggregate(control);
    }
}

public class SurveyAggregate
{
    public Survey Survey { get; }

    public SurveyAggregate(Survey survey)
    {
        Survey = survey;
    }

    public void AddQuestion(SurveyQuestion question)
    {
        Survey.Questions.Add(question);
    }

    public void SubmitFeedback(SurveyQuestion question, SurveyFeedback feedback)
    {
        question.Feedbacks.Add(feedback);
        question.AddDomainEvent(new SurveyFeedbackSubmittedEvent(feedback.FeedbackId, question.SurveyQuestionId, feedback.Status));
    }

    public static SurveyAggregate Create(Survey survey)
    {
        survey.AddDomainEvent(new SurveyCreatedEvent(survey.SurveyId, survey.Title));
        return new SurveyAggregate(survey);
    }
}
