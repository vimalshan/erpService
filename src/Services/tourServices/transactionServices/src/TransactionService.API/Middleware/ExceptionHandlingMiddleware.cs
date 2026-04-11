using System.Net;
using System.Text.Json;
using TransactionService.Domain.Exceptions;

namespace TransactionService.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch
        {
            JournalVoucherNotFoundException => (HttpStatusCode.NotFound, "Journal Voucher Not Found"),
            JournalVoucherAlreadyPostedException => (HttpStatusCode.Conflict, "Journal Voucher Already Posted"),
            TravelBatchNotFoundException => (HttpStatusCode.NotFound, "Travel Batch Not Found"),
            TravelBatchInvalidStateException => (HttpStatusCode.UnprocessableEntity, "Invalid Batch State"),
            EmployeePaymentNotFoundException => (HttpStatusCode.NotFound, "Employee Payment Not Found"),
            AirlineInvoiceNotFoundException => (HttpStatusCode.NotFound, "Airline Invoice Not Found"),
            DomainException => (HttpStatusCode.UnprocessableEntity, "Domain Error"),
            FluentValidation.ValidationException => (HttpStatusCode.BadRequest, "Validation Failed"),
            InvalidOperationException => (HttpStatusCode.Conflict, "Operation Failed"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        var errors = ex is FluentValidation.ValidationException ve
            ? ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToArray<object>()
            : [new { Message = ex.Message }];

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var problem = new
        {
            title,
            status = (int)status,
            detail = ex.Message,
            errors
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
