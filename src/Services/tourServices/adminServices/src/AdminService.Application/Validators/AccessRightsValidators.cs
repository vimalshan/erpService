using FluentValidation;
using AdminService.Application.Commands.AccessRights;

namespace AdminService.Application.Validators;

public class CreateAccessRightsCommandValidator : AbstractValidator<CreateAccessRightsCommand>
{
    public CreateAccessRightsCommandValidator()
    {
        RuleFor(x => x.AdminRightsId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminLocationId).MaximumLength(255);
        RuleFor(x => x.AdminRightsFor).MaximumLength(255);
        RuleFor(x => x.AdminRightsType).MaximumLength(255);
        RuleFor(x => x.AdminUserId).MaximumLength(255);
        RuleFor(x => x.AdminContactNo).MaximumLength(255);
    }
}
