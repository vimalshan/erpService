using ApiGateway.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace ApiGateway.Gateway;

[ApiController]
[Route("{serviceName}")]
public class ProxyController : ControllerBase
{
    private readonly GatewayConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProxyController> _logger;

    public ProxyController(
        GatewayConfiguration config,
        IHttpClientFactory httpClientFactory,
        ILogger<ProxyController> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Proxy GET requests to downstream services.
    /// Route: /{serviceName}/{**path} → downstream /api/{path}
    /// </summary>
    [HttpGet("{**path}")]
    [Authorize]
    public Task<IActionResult> ProxyGet(string serviceName, string? path) =>
        ProxyRequest(serviceName, path);

    /// <summary>
    /// Proxy POST requests to downstream services
    /// </summary>
    [HttpPost("{**path}")]
    [Authorize]
    public Task<IActionResult> ProxyPost(string serviceName, string? path) =>
        ProxyRequest(serviceName, path);

    /// <summary>
    /// Proxy PUT requests to downstream services
    /// </summary>
    [HttpPut("{**path}")]
    [Authorize]
    public Task<IActionResult> ProxyPut(string serviceName, string? path) =>
        ProxyRequest(serviceName, path);

    /// <summary>
    /// Proxy DELETE requests to downstream services
    /// </summary>
    [HttpDelete("{**path}")]
    [Authorize]
    public Task<IActionResult> ProxyDelete(string serviceName, string? path) =>
        ProxyRequest(serviceName, path);

    /// <summary>
    /// Proxy PATCH requests to downstream services
    /// </summary>
    [HttpPatch("{**path}")]
    [Authorize]
    public Task<IActionResult> ProxyPatch(string serviceName, string? path) =>
        ProxyRequest(serviceName, path);

    private async Task<IActionResult> ProxyRequest(string serviceName, string? path)
    {
        var service = _config.Services.FirstOrDefault(
            s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

        if (service == null)
            return NotFound(new { error = $"Service '{serviceName}' not found", availableServices = _config.Services.Select(s => s.Name.ToLower()) });

        var client = _httpClientFactory.CreateClient(service.Name);
        var downstreamPath = string.IsNullOrEmpty(path) ? "/api" : $"/api/{path}";

        // Forward query string
        if (Request.QueryString.HasValue)
            downstreamPath += Request.QueryString.Value;

        _logger.LogInformation("Proxying {Method} /{Service}/{Path} → {Downstream}",
            Request.Method, serviceName, path, downstreamPath);

        try
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(Request.Method), downstreamPath);

            // Forward request body for POST/PUT/PATCH
            if (Request.ContentLength > 0 && (Request.Method == "POST" || Request.Method == "PUT" || Request.Method == "PATCH"))
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
                requestMessage.Content = new StringContent(body, Encoding.UTF8);
                if (Request.ContentType != null)
                    requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(Request.ContentType);
            }

            // Forward selected headers
            foreach (var header in new[] { "Accept", "Accept-Language", "X-Correlation-ID" })
            {
                if (Request.Headers.TryGetValue(header, out var values))
                    requestMessage.Headers.TryAddWithoutValidation(header, values.ToArray());
            }

            // Forward auth header to downstream
            if (Request.Headers.TryGetValue("Authorization", out var authValues))
                requestMessage.Headers.TryAddWithoutValidation("Authorization", authValues.ToArray());

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            // Forward response content type
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = contentType
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to proxy request to {Service}", serviceName);
            return StatusCode(502, new
            {
                error = "Bad Gateway",
                message = $"Failed to reach downstream service '{serviceName}'",
                detail = ex.Message
            });
        }
        catch (TaskCanceledException)
        {
            return StatusCode(504, new
            {
                error = "Gateway Timeout",
                message = $"Request to '{serviceName}' timed out"
            });
        }
    }
}
