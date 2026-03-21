using ExpenseService.Application.DTOs;

namespace ExpenseService.API.GraphQL.Types;

public class ExpenseType : ObjectType<TravelExpenseDto>
{
    protected override void Configure(IObjectTypeDescriptor<TravelExpenseDto> descriptor)
    {
        descriptor.Name("TravelExpense");
        descriptor.Field(e => e.RequestNumber).Description("Travel request number");
        descriptor.Field(e => e.SerialNumber).Description("Expense serial number");
        descriptor.Field(e => e.ExpenseCode).Description("Expense category code");
        descriptor.Field(e => e.BudgetAmount).Description("Budgeted amount");
        descriptor.Field(e => e.EligibleAmount).Description("Eligible/actual amount");
        descriptor.Field(e => e.SelfExpense).Description("Amount met by employee");
        descriptor.Field(e => e.VarianceAmount).Description("Variance between budget and actual");
        descriptor.Field(e => e.Allocations).Description("Cost allocations");
        descriptor.Field(e => e.SubDetails).Description("Sub expense details");
    }
}
