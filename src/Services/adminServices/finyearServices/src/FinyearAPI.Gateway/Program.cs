using FinyearAPI.Gateway.Middleware;
using FinyearAPI.Gateway.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapApiGatewayRoutes();

app.Run();
