using FluentValidation;
using AdminService.Application.Commands.AdminMasters;

namespace AdminService.Application.Validators;

public class CreateAdminMasterCommandValidator : AbstractValidator<CreateAdminMasterCommand>
{
    public CreateAdminMasterCommandValidator()
    {
        RuleFor(x => x.AdminId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminPic).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminUnitId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminUnitHeadSysId).NotEmpty().MaximumLength(255);
    }
}

public class UpdateAdminMasterCommandValidator : AbstractValidator<UpdateAdminMasterCommand>
{
    public UpdateAdminMasterCommandValidator()
    {
        RuleFor(x => x.AdminId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminPic).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminUnitId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminUnitHeadSysId).NotEmpty().MaximumLength(255);
    }
}
