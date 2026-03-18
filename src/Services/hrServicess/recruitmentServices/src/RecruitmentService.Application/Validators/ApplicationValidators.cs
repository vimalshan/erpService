using FluentValidation;
using RecruitmentService.Application.Commands.Applications;
using RecruitmentService.Application.Commands.Prospects;

namespace RecruitmentService.Application.Validators;

public class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
{
    public SubmitApplicationCommandValidator()
    {
        RuleFor(x => x.Request.AppId).GreaterThan(0);
        RuleFor(x => x.Request.AppSl).GreaterThan(0);
        RuleFor(x => x.Request.VacancyId).GreaterThan(0);
        RuleFor(x => x.SubmittedBy).GreaterThan(0);
    }
}

public class RegisterProspectCommandValidator : AbstractValidator<RegisterProspectCommand>
{
    public RegisterProspectCommandValidator()
    {
        RuleFor(x => x.Request.UserId).GreaterThan(0);
        RuleFor(x => x.Request.FirstName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.Request.LastName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.Request.EmailId).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.");
    }
}
