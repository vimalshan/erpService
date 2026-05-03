using HotChocolate;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AuditService.GraphQL;

public class GraphQLErrorFilter : IErrorFilter
{
    private readonly ILogger<GraphQLErrorFilter> _logger;
    public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger) { _logger = logger; }

    public IError OnError(IError error)
    {
        var ex = error.Exception;
        if (ex is null) return error;

        _logger.LogError(ex, "GraphQL error at {Path}: {Message}", error.Path, ex.Message);

        return ex switch
        {
            System.Collections.Generic.KeyNotFoundException => error.WithMessage(ex.Message).WithCode("NOT_FOUND"),
            ArgumentException ae => error.WithMessage(ae.Message).WithCode("BAD_REQUEST"),
            InvalidOperationException ioe => error.WithMessage(ioe.Message).WithCode("INVALID_OPERATION"),
            UnauthorizedAccessException => error.WithMessage("Unauthorized").WithCode("UNAUTHORIZED"),
            SqlException se => error.WithMessage($"Database error: {se.Message}").WithCode("DATABASE_ERROR"),
            _ => error.WithMessage(ex.Message).WithCode("INTERNAL_ERROR")
        };
    }
}
