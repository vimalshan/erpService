using ApiGateway.GraphQL;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphQLProxyController : ControllerBase
{
    private readonly GraphQLSchemaProxy _proxy;
    private readonly ILogger<GraphQLProxyController> _logger;

    public GraphQLProxyController(GraphQLSchemaProxy proxy, ILogger<GraphQLProxyController> logger)
    {
        _proxy = proxy;
        _logger = logger;
    }

    /// <summary>
    /// Forward a GraphQL query to a specific downstream service.
    /// </summary>
    [HttpPost("{serviceName}")]
    public async Task<IActionResult> ForwardGraphQL(string serviceName, [FromBody] GraphQLRequest request)
    {
        try
        {
            var result = await _proxy.ForwardQueryAsync(serviceName, request.Query, request.Variables);
            return Content(result, "application/json");
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error forwarding GraphQL to {Service}", serviceName);
            return StatusCode(502, new { error = $"Error reaching service '{serviceName}'" });
        }
    }
}

public class GraphQLRequest
{
    public string Query { get; set; } = string.Empty;
    public string? Variables { get; set; }
}
