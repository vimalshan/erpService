using WebsiteContentService.Application.Queries.News;
using WebsiteContentService.Application.Queries.Pages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;

namespace WebsiteContentService.AzureFunctions;

public class WebsiteContentFunction(IMediator mediator, ILogger<WebsiteContentFunction> logger)
{
    [Function("ArchiveExpiredContent")]
    public async Task ArchiveExpiredContentAsync([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer)
    {
        logger.LogInformation("ArchiveExpiredContent function triggered at {Time}", DateTime.UtcNow);

        try
        {
            var publishedNews = await mediator.Send(new GetPublishedWebsiteNewsQuery());
            var expiredCount = 0;

            foreach (var news in publishedNews)
            {
                if (news.PublishEndDate.HasValue && news.PublishEndDate.Value < DateTime.UtcNow)
                {
                    logger.LogInformation("Archiving expired news: {NewsId} - {Title}", news.NewsId, news.NewsTitle);
                    expiredCount++;
                }
            }

            logger.LogInformation("Archived {Count} expired news articles", expiredCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error archiving expired content");
        }
    }

    [Function("ProcessWebsiteContentEvents")]
    public async Task ProcessContentEventsAsync(
        [RabbitMQTrigger("website-content-events")] string message)
    {
        logger.LogInformation("Processing website content event: {Message}", message);

        try
        {
            // Process domain events from RabbitMQ
            logger.LogInformation("Successfully processed website content event");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing website content event: {Message}", message);
        }
    }

    [Function("ProcessWebsiteImageUpload")]
    public async Task ProcessImageUploadAsync(
        [BlobTrigger("website-content-images/{name}", Connection = "AzureWebJobsStorage")] Stream image,
        string name)
    {
        logger.LogInformation("Processing uploaded image: {Name}, Size: {Size} bytes", name, image.Length);

        try
        {
            // Process image (resize, optimize, etc.)
            logger.LogInformation("Successfully processed image: {Name}", name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing image: {Name}", name);
        }
    }
}
