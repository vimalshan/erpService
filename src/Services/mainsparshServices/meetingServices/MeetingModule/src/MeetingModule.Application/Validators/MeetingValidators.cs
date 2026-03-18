using FluentValidation;
using MeetingModule.Application.Commands.Meetings;

namespace MeetingModule.Application.Validators;

public class CreateMeetingValidator : AbstractValidator<CreateMeetingCommand>
{
    public CreateMeetingValidator()
    {
        RuleFor(x => x.Dto.MeetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.MeetingTitle).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Dto.MeetingDate).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.Dto.OrganizerId).GreaterThan(0);
        RuleFor(x => x.Dto.MeetingLocation).MaximumLength(255);
        RuleFor(x => x.Dto.MeetingDuration).GreaterThan(0).When(x => x.Dto.MeetingDuration.HasValue);
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}

public class UpdateMeetingValidator : AbstractValidator<UpdateMeetingCommand>
{
    public UpdateMeetingValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.MeetingTitle).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Dto.MeetingDate).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.Dto.MeetingLocation).MaximumLength(255);
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}
