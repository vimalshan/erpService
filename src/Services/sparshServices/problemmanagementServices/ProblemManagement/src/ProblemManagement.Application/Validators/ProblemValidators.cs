using FluentValidation;
using ProblemManagement.Application.Commands;

namespace ProblemManagement.Application.Validators;

public class CreateProblemValidator : AbstractValidator<CreateProblemCommand>
{
    public CreateProblemValidator()
    {
        RuleFor(x => x.Owner).GreaterThan(0).WithMessage("Owner is required.");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.SiteId).GreaterThan(0);
        RuleFor(x => x.EnteredBy).GreaterThan(0);
    }
}

public class ApproveProblemValidator : AbstractValidator<ApproveProblemCommand>
{
    public ApproveProblemValidator()
    {
        RuleFor(x => x.ProblemId).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
        RuleFor(x => x.Status).Must(s => s is "A" or "R").WithMessage("Status must be A or R.");
    }
}

public class RecordSolutionValidator : AbstractValidator<RecordSolutionCommand>
{
    public RecordSolutionValidator()
    {
        RuleFor(x => x.ProblemId).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.EnteredBy).GreaterThan(0);
    }
}

public class AddSolutionCommentValidator : AbstractValidator<AddSolutionCommentCommand>
{
    public AddSolutionCommentValidator()
    {
        RuleFor(x => x.SolutionId).GreaterThan(0);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        RuleFor(x => x.CommentBy).GreaterThan(0);
    }
}
