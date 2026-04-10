using FluentValidation;
using TaskTransactional.Application.Commands;

namespace TaskTransactional.Application.Validators;

public class CreateComplaintMainValidator : AbstractValidator<CreateComplaintMainCommand>
{
    public CreateComplaintMainValidator()
    {
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.GroupSrc).GreaterThan(0);
    }
}

public class UpdateComplaintMainValidator : AbstractValidator<UpdateComplaintMainCommand>
{
    public UpdateComplaintMainValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.UpdatedBy).NotEmpty().MaximumLength(255);
    }
}

public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.GroupId).GreaterThan(0);
        RuleFor(x => x.Type).GreaterThan(0);
        RuleFor(x => x.Location).GreaterThan(0);
        RuleFor(x => x.Department).GreaterThan(0);
        RuleFor(x => x.Process).GreaterThan(0);
        RuleFor(x => x.TargetDate).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Subject).MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(4000);
        RuleFor(x => x.Ncr).Must(x => x is null or "Y" or "N");
    }
}

public class CreateComplaintTaskValidator : AbstractValidator<CreateComplaintTaskCommand>
{
    public CreateComplaintTaskValidator()
    {
        RuleFor(x => x.TicketNum).GreaterThan(0);
        RuleFor(x => x.ScheduleFreq).NotEmpty().MaximumLength(2);
    }
}

public class UpdatePrimaryActionValidator : AbstractValidator<UpdatePrimaryActionCommand>
{
    public UpdatePrimaryActionValidator()
    {
        RuleFor(x => x.ActionNum).GreaterThan(0);
        RuleFor(x => x.ActBy).GreaterThan(0);
        RuleFor(x => x.Solution).MaximumLength(4000);
    }
}

public class CreateEscalationValidator : AbstractValidator<CreateEscalationCommand>
{
    public CreateEscalationValidator()
    {
        RuleFor(x => x.TicketNum).GreaterThan(0);
        RuleFor(x => x.LevelNum).GreaterThan(0);
        RuleFor(x => x.EscNoHrs).GreaterThan(0);
        RuleFor(x => x.UserPin).GreaterThan(0);
    }
}

public class CreateHistoryValidator : AbstractValidator<CreateHistoryCommand>
{
    public CreateHistoryValidator()
    {
        RuleFor(x => x.ActionNum).GreaterThan(0);
        RuleFor(x => x.From).NotEmpty().MaximumLength(65);
        RuleFor(x => x.To).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ActionType).NotEmpty().MaximumLength(1);
    }
}
