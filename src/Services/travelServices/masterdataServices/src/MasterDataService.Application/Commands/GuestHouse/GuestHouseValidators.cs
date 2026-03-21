using FluentValidation;

namespace MasterDataService.Application.Commands.GuestHouse;

public class CreateGuestHouseCommandValidator : AbstractValidator<CreateGuestHouseCommand>
{
    public CreateGuestHouseCommandValidator()
    {
        RuleFor(x => x.AdminCode).GreaterThan(0);
        RuleFor(x => x.GuestHouseName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DailyAmount).GreaterThanOrEqualTo(0);
    }
}

public class UpdateGuestHouseCommandValidator : AbstractValidator<UpdateGuestHouseCommand>
{
    public UpdateGuestHouseCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.GuestHouseName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DailyAmount).GreaterThanOrEqualTo(0);
    }
}
