using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmployeeService.AzureFunctions.Functions;

public class EmployeeImageProcessorFunction
{
    private readonly ILogger<EmployeeImageProcessorFunction> _logger;

    public EmployeeImageProcessorFunction(ILogger<EmployeeImageProcessorFunction> logger)
    {
        _logger = logger;
    }

    [Function("EmployeeImageProcessor")]
    public async Task Run(
        [BlobTrigger("employee-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream blobStream,
        string name)
    {
        _logger.LogInformation("Processing employee image: {Name}", name);

        // Placeholder: Implement image processing logic such as resizing,
        // thumbnail generation, metadata extraction, etc.
        await Task.CompletedTask;

        _logger.LogInformation("Employee image processed: {Name}", name);
    }
}
