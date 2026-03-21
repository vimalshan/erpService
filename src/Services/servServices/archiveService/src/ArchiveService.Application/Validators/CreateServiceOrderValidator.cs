using ArchiveService.Application.Features.ServiceOrders.Commands;
using FluentValidation;

namespace ArchiveService.Application.Validators;

public class CreateServiceOrderValidator : AbstractValidator<CreateServiceOrderCommand>
{
    public CreateServiceOrderValidator()
    {
        RuleFor(x => x.SernoDell)
            .NotEmpty().WithMessage("SERNO_DELL is required")
            .MaximumLength(12);

        RuleFor(x => x.Branch).MaximumLength(15);
        RuleFor(x => x.SapId).MaximumLength(12);
        RuleFor(x => x.ServiceTag).MaximumLength(25);
        RuleFor(x => x.CustomerName).MaximumLength(25);
        RuleFor(x => x.ContactNo).MaximumLength(15);
    }
}
