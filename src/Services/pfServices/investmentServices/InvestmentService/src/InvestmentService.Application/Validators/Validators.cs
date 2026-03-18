using FluentValidation;
using InvestmentService.Application.Commands;

namespace InvestmentService.Application.Validators;

public class CreateInvestmentCommandValidator : AbstractValidator<CreateInvestmentCommand>
{
    public CreateInvestmentCommandValidator()
    {
        RuleFor(x => x.InvNo).GreaterThan(0).WithMessage("Investment number is required");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category is required");
        RuleFor(x => x.Units).GreaterThan(0).WithMessage("Units must be greater than 0");
        RuleFor(x => x.PurchaseRate).GreaterThan(0).WithMessage("Purchase rate must be greater than 0");
        RuleFor(x => x.PurchaseDate).NotEmpty().WithMessage("Purchase date is required");
        RuleFor(x => x.MaturityDate).GreaterThan(x => x.PurchaseDate).WithMessage("Maturity date must be after purchase date");
        RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0).WithMessage("Interest rate cannot be negative");
        RuleFor(x => x.EnteredBy).GreaterThan(0).WithMessage("Entered by is required");
    }
}

public class RedeemInvestmentCommandValidator : AbstractValidator<RedeemInvestmentCommand>
{
    public RedeemInvestmentCommandValidator()
    {
        RuleFor(x => x.SaleNo).GreaterThan(0);
        RuleFor(x => x.InvNo).GreaterThan(0);
        RuleFor(x => x.SaleType).NotEmpty().MaximumLength(1);
        RuleFor(x => x.SaleDate).NotEmpty();
        RuleFor(x => x.SaleValue).GreaterThan(0);
        RuleFor(x => x.EnteredBy).GreaterThan(0);
    }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Code).GreaterThan(0);
        RuleFor(x => x.ShortCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Denomination).GreaterThan(0);
        RuleFor(x => x.GroupId).GreaterThan(0);
    }
}

public class CreateBrokerCommandValidator : AbstractValidator<CreateBrokerCommand>
{
    public CreateBrokerCommandValidator()
    {
        RuleFor(x => x.BrokerId).GreaterThan(0);
        RuleFor(x => x.BrokerName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BrokerStatus).NotEmpty().MaximumLength(1);
    }
}
