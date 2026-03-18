using FluentValidation;
using MeetingModule.Application.Commands.MeetingTypes;

namespace MeetingModule.Application.Validators;

public class CreateMeetingTypeValidator : AbstractValidator<CreateMeetingTypeCommand>
{
    public CreateMeetingTypeValidator()
    {
        RuleFor(x => x.Dto.MeetTypeCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.MeetTypeName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}

public class UpdateMeetingTypeValidator : AbstractValidator<UpdateMeetingTypeCommand>
{
    public UpdateMeetingTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.MeetTypeName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}
