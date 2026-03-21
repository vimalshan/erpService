using FluentValidation;
using AdminService.Application.Commands.UserMaps;

namespace AdminService.Application.Validators;

public class CreateAdminUserMapCommandValidator : AbstractValidator<CreateAdminUserMapCommand>
{
    private static readonly HashSet<string> ValidBookTypes = new() { "TKT", "STY", "CAB", "FRX" };

    public CreateAdminUserMapCommandValidator()
    {
        RuleFor(x => x.AdminMapId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminBookType).NotEmpty().MaximumLength(255)
            .Must(v => ValidBookTypes.Contains(v)).WithMessage("BookType must be TKT, STY, CAB, or FRX.");
        RuleFor(x => x.AdminMode).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminEmpSysId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminLastModifiedBy).NotEmpty().MaximumLength(255);
    }
}
