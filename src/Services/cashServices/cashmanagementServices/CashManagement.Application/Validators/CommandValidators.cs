using FluentValidation;
using CashManagement.Application.Commands.CashTransaction;
using CashManagement.Application.Commands.ChequeRegister;
using CashManagement.Application.Commands.BankReconciliation;
using CashManagement.Application.Commands.CashUnit;
using CashManagement.Application.Commands.BankAccount;
using CashManagement.Application.Commands.BankTransaction;

namespace CashManagement.Application.Validators;

public class CreateCashUnitValidator : AbstractValidator<CreateCashUnitCommand>
{
    public CreateCashUnitValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.OpeningBalance).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class RecordCashReceiptValidator : AbstractValidator<RecordCashReceiptCommand>
{
    public RecordCashReceiptValidator()
    {
        RuleFor(x => x.CashUnitId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Receipt amount must be greater than zero.");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class RecordCashDisbursementValidator : AbstractValidator<RecordCashDisbursementCommand>
{
    public RecordCashDisbursementValidator()
    {
        RuleFor(x => x.CashUnitId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Disbursement amount must be greater than zero.");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountValidator()
    {
        RuleFor(x => x.BankName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AccountNo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class RecordBankTransactionValidator : AbstractValidator<RecordBankTransactionCommand>
{
    public RecordBankTransactionValidator()
    {
        RuleFor(x => x.BankAccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class IssueChequeValidator : AbstractValidator<IssueChequeCommand>
{
    public IssueChequeValidator()
    {
        RuleFor(x => x.BankAccountId).GreaterThan(0);
        RuleFor(x => x.ChequeNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PayeeName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.ChequeDate).NotEmpty();
        RuleFor(x => x.IssuedBy).GreaterThan(0);
    }
}

public class MarkChequeBouncedValidator : AbstractValidator<MarkChequeBouncedCommand>
{
    public MarkChequeBouncedValidator()
    {
        RuleFor(x => x.ChequeId).GreaterThan(0);
        RuleFor(x => x.BounceReason).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ProcessedBy).GreaterThan(0);
    }
}

public class PerformBankReconciliationValidator : AbstractValidator<PerformBankReconciliationCommand>
{
    public PerformBankReconciliationValidator()
    {
        RuleFor(x => x.BankAccountId).GreaterThan(0);
        RuleFor(x => x.ReconciliationDate).NotEmpty();
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
