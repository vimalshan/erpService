namespace FinyearAPI.Gateway.Routing
{
    /// <summary>
    /// API Gateway routing configuration
    /// Routes requests to appropriate microservices
    /// </summary>
    public static class GatewayRouting
    {
        /// <summary>
        /// Configure API Gateway routes
        /// </summary>
        public static void MapApiGatewayRoutes(this WebApplication app)
        {
            // Financial Year Management Service Routes
            MapFinancialYearRoutes(app);

            // Health check endpoint
            app.MapGet("/health", HealthCheckHandler).WithName("Health").WithOpenApi();

            // API Gateway info
            app.MapGet("/api/gateway/info", GetGatewayInfo).WithName("GatewayInfo").WithOpenApi();
        }

        private static void MapFinancialYearRoutes(WebApplication app)
        {
            const string baseRoute = "/api/v{version:apiVersion}/financialyear";

            // GET all financial years
            app.MapGet($"{baseRoute}", GetAllFinancialYears)
                .WithName("GetAllFinancialYears")
                .WithOpenApi()
                .RequireAuthorization("UserOrAdmin");

            // GET financial year by ID
            app.MapGet($"{baseRoute}/{{id}}", GetFinancialYearById)
                .WithName("GetFinancialYearById")
                .WithOpenApi()
                .RequireAuthorization("UserOrAdmin");

            // GET current financial year
            app.MapGet($"{baseRoute}/current", GetCurrentFinancialYear)
                .WithName("GetCurrentFinancialYear")
                .WithOpenApi();

            // GET financial year by name
            app.MapGet($"{baseRoute}/by-name/{{name}}", GetFinancialYearByName)
                .WithName("GetFinancialYearByName")
                .WithOpenApi()
                .RequireAuthorization("UserOrAdmin");

            // POST create financial year
            app.MapPost($"{baseRoute}", CreateFinancialYear)
                .WithName("CreateFinancialYear")
                .WithOpenApi()
                .RequireAuthorization("AdminOnly");

            // PUT update financial year
            app.MapPut($"{baseRoute}/{{id}}", UpdateFinancialYear)
                .WithName("UpdateFinancialYear")
                .WithOpenApi()
                .RequireAuthorization("AdminOnly");

            // DELETE financial year
            app.MapDelete($"{baseRoute}/{{id}}", DeleteFinancialYear)
                .WithName("DeleteFinancialYear")
                .WithOpenApi()
                .RequireAuthorization("AdminOnly");
        }

        #region Handlers

        private static IResult GetAllFinancialYears(HttpContext context, ILogger<object> logger)
        {
            logger.LogInformation("GET: All financial years");
            return Results.Ok(new { message = "Financial years retrieved" });
        }

        private static IResult GetFinancialYearById(HttpContext context, long id, ILogger<object> logger)
        {
            logger.LogInformation("GET: Financial year by ID: {Id}", id);
            return Results.Ok(new { id, message = "Financial year retrieved" });
        }

        private static IResult GetCurrentFinancialYear(HttpContext context, ILogger<object> logger)
        {
            logger.LogInformation("GET: Current financial year");
            return Results.Ok(new { message = "Current financial year retrieved" });
        }

        private static IResult GetFinancialYearByName(HttpContext context, string name, ILogger<object> logger)
        {
            logger.LogInformation("GET: Financial year by name: {Name}", name);
            return Results.Ok(new { name, message = "Financial year retrieved" });
        }

        private static IResult CreateFinancialYear(HttpContext context, CreateFinancialYearRequest request, ILogger<object> logger)
        {
            logger.LogInformation("POST: Creating financial year: {Name}", request.Name);
            return Results.Created($"/api/financialyear/1", new { id = 1, name = request.Name });
        }

        private static IResult UpdateFinancialYear(HttpContext context, long id, UpdateFinancialYearRequest request, ILogger<object> logger)
        {
            logger.LogInformation("PUT: Updating financial year: {Id}", id);
            return Results.Ok(new { id, message = "Financial year updated" });
        }

        private static IResult DeleteFinancialYear(HttpContext context, long id, ILogger<object> logger)
        {
            logger.LogInformation("DELETE: Deleting financial year: {Id}", id);
            return Results.NoContent();
        }

        private static IResult HealthCheckHandler(ILogger<object> logger)
        {
            logger.LogInformation("Health check requested");
            return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }

        private static IResult GetGatewayInfo(ILogger<object> logger)
        {
            logger.LogInformation("Gateway info requested");
            return Results.Ok(new
            {
                service = "FinyearAPI Gateway",
                version = "1.0.0",
                timestamp = DateTime.UtcNow,
                features = new[] { "REST API", "GraphQL", "WebSocket", "Circuit Breaker", "CQRS" }
            });
        }

        #endregion
    }

    /// <summary>
    /// Request models for gateway
    /// </summary>
    public class CreateFinancialYearRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateFinancialYearRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
