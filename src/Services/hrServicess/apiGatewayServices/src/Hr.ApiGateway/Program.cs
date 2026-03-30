using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Hr.ApiGateway.Health;
using Hr.ApiGateway.Middleware;
using Hr.ApiGateway.Telemetry;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GatewayMetrics>();
builder.Services.AddHttpClient("gateway-health");

builder.Services.AddResponseCaching();

builder.Services.AddHealthChecks()
	.AddCheck<DownstreamServicesHealthCheck>("downstream-services");

var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("JWT secret is missing for gateway authentication.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("GatewayAccess", policy => policy.RequireAuthenticatedUser());
});

var rateLimitSection = builder.Configuration.GetSection("RateLimiting");
var permitLimit = rateLimitSection.GetValue<int?>("PermitLimit") ?? 100;
var windowSeconds = rateLimitSection.GetValue<int?>("WindowSeconds") ?? 60;
var queueLimit = rateLimitSection.GetValue<int?>("QueueLimit") ?? 200;

builder.Services.AddRateLimiter(options =>
{
	options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
	{
		if (context.Request.Path.StartsWithSegments("/health") || context.Request.Path.StartsWithSegments("/metrics"))
		{
			return RateLimitPartition.GetNoLimiter("health-and-metrics");
		}

		var key = context.User.Identity?.IsAuthenticated == true
			? context.User.Identity.Name ?? "authenticated"
			: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

		return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
		{
			PermitLimit = permitLimit,
			QueueLimit = queueLimit,
			QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
			Window = TimeSpan.FromSeconds(windowSeconds),
			AutoReplenishment = true
		});
	});
});

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<BulkheadIsolationMiddleware>();
app.UseMiddleware<GatewayResilienceMiddleware>();

app.UseRateLimiter();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok(new
{
	Name = "HR API Gateway",
	Proxy = "YARP",
	Version = "v1"
}));

app.MapPost("/gateway/auth/login", (GatewayLoginRequest request) =>
{
	var users = new Dictionary<string, (string Password, string[] Roles)>(StringComparer.OrdinalIgnoreCase)
	{
		["admin"] = ("admin123", ["Admin", "GatewayUser"]),
		["manager"] = ("manager123", ["Manager", "GatewayUser"]),
		["reader"] = ("reader123", ["Reader", "GatewayUser"])
	};

	if (!users.TryGetValue(request.Username, out var user) || user.Password != request.Password)
	{
		return Results.Unauthorized();
	}

	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
	var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
	var expires = DateTime.UtcNow.AddHours(8);

	var claims = new List<Claim>
	{
		new(ClaimTypes.Name, request.Username),
		new(JwtRegisteredClaimNames.Sub, request.Username),
		new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
	};

	claims.AddRange(user.Roles.Select(static role => new Claim(ClaimTypes.Role, role)));

	var token = new JwtSecurityToken(
		issuer: jwtSection["Issuer"],
		audience: jwtSection["Audience"],
		claims: claims,
		expires: expires,
		signingCredentials: creds);

	var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
	return Results.Ok(new { accessToken = tokenString, expiresAtUtc = expires });
})
.AllowAnonymous();

app.MapHealthChecks("/health");

app.MapGet("/metrics", (GatewayMetrics metrics) => Results.Ok(metrics.Snapshot()));

app.MapReverseProxy(proxyPipeline =>
{
	proxyPipeline.Use(async (context, next) =>
	{
		await next().ConfigureAwait(false);

		if (HttpMethods.IsGet(context.Request.Method) && context.Response.StatusCode == StatusCodes.Status200OK)
		{
			if (!context.Response.Headers.ContainsKey("Cache-Control"))
			{
				context.Response.Headers.CacheControl = "public,max-age=30";
			}
		}
	});
})
.RequireAuthorization("GatewayAccess");

app.Run();

public sealed record GatewayLoginRequest(string Username, string Password);
