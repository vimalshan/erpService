using CSA.Service.Application.Commands.Controls;
using CSA.Service.Application.Commands.Surveys;
using CSA.Service.Application.Commands.Processes;
using FluentValidation;

namespace CSA.Service.Application.Common;

public class CreateControlCommandValidator : AbstractValidator<CreateControlCommand>
{
    public CreateControlCommandValidator()
    {
        RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(2000);
        RuleFor(x => x.Dto.Risk).MaximumLength(2000);
        RuleFor(x => x.Dto.ControlType).Must(v => v is null or "P" or "D")
            .WithMessage("ControlType must be P (Preventative) or D (Detective).");
        RuleFor(x => x.Dto.Priority).Must(v => v is null or "H" or "M" or "L")
            .WithMessage("Priority must be H, M, or L.");
        RuleFor(x => x.Dto.Periodicity).Must(v => v is null or "M" or "Q" or "A")
            .WithMessage("Periodicity must be M, Q, or A.");
    }
}

public class CreateSurveyCommandValidator : AbstractValidator<CreateSurveyCommand>
{
    public CreateSurveyCommandValidator()
    {
        RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Dto.DueDate).NotEmpty();
        RuleFor(x => x.Dto.CloseDate).NotEmpty().GreaterThanOrEqualTo(x => x.Dto.DueDate);
        RuleFor(x => x.Dto.StartDate).NotEmpty();
        RuleFor(x => x.Dto.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.Dto.StartDate);
    }
}

public class CreateProcessCommandValidator : AbstractValidator<CreateProcessCommand>
{
    public CreateProcessCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(2000);
    }
}
