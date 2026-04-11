using TransactionService.Domain.Exceptions;

namespace TransactionService.API.GraphQL;

public class GraphQLErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is DomainException domainEx)
        {
            return ErrorBuilder.FromError(error)
                .SetMessage(domainEx.Message)
                .SetCode(error.Exception switch
                {
                    JournalVoucherNotFoundException or
                    TravelBatchNotFoundException or
                    EmployeePaymentNotFoundException or
                    AirlineInvoiceNotFoundException => "NOT_FOUND",
                    _ => "DOMAIN_ERROR"
                })
                .RemoveException()
                .Build();
        }

        if (error.Exception is ArgumentException argEx)
        {
            return ErrorBuilder.FromError(error)
                .SetMessage(argEx.Message)
                .SetCode("VALIDATION_ERROR")
                .RemoveException()
                .Build();
        }

        if (error.Exception is InvalidOperationException invOpEx)
        {
            return ErrorBuilder.FromError(error)
                .SetMessage(invOpEx.Message)
                .SetCode("OPERATION_FAILED")
                .RemoveException()
                .Build();
        }

        return error;
    }
}
