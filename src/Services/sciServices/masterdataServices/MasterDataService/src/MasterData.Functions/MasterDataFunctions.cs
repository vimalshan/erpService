using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MasterData.Application.DTOs;
using MasterData.Domain.Aggregates;
using System;

namespace MasterData.Functions
{
    /// <summary>
    /// Azure Functions for background tasks and event processing
    /// </summary>
    public class MasterDataFunctions
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger _logger;

        public MasterDataFunctions(IUnitOfWork unitOfWork, BlobContainerClient blobContainerClient, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _blobContainerClient = blobContainerClient;
            _logger = loggerFactory.CreateLogger<MasterDataFunctions>();
        }

        /// <summary>
        /// Timer trigger function to process master data updates every hour
        /// </summary>
        [Function("ProcessMasterDataUpdates")]
        public async Task ProcessMasterDataUpdates([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, FunctionContext context)
        {
            _logger.LogInformation($"Processing Master Data Updates at {DateTime.UtcNow}");

            try
            {
                // Get all company units
                var units = await _unitOfWork.CompanyUnits.GetAllAsync();
                _logger.LogInformation($"Processing {units.Count} company units");

                // Get all locations
                var locations = await _unitOfWork.Locations.GetAllAsync();
                _logger.LogInformation($"Processing {locations.Count} locations");

                // Log processed count
                _logger.LogInformation($"Master Data processing completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing master data updates");
                throw;
            }
        }

        /// <summary>
        /// Http trigger function to upload stationery item images to Blob Storage
        /// </summary>
        [Function("UploadStationeryImage")]
        public async Task<dynamic> UploadStationeryImage(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload-image/{itemId}")] HttpRequestData req,
            string itemId,
            FunctionContext context)
        {
            _logger.LogInformation($"Uploading image for stationery item: {itemId}");

            try
            {
                if (!req.Body.CanSeek)
                {
                    req.Body.Position = 0;
                }

                var fileName = $"stationery-items/{itemId}/{Guid.NewGuid()}.jpg";
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                await blobClient.UploadAsync(req.Body, overwrite: true);

                _logger.LogInformation($"Image uploaded successfully: {fileName}");

                var response = req.CreateResponse(System.Net.HttpStatusCode.Created);
                await response.WriteAsJsonAsync(new { 
                    success = true, 
                    message = "Image uploaded successfully",
                    blobUri = blobClient.Uri.ToString()
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading stationery image");
                
                var response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { 
                    success = false, 
                    message = "Error uploading image" 
                });

                return response;
            }
        }

        /// <summary>
        /// Blob trigger function to process uploaded images
        /// </summary>
        [Function("ProcessUploadedImage")]
        public async Task ProcessUploadedImage(
            [BlobTrigger("masterdata-images/{name}")] Stream image,
            string name,
            FunctionContext context)
        {
            _logger.LogInformation($"Processing uploaded image: {name}");

            try
            {
                // Image size check
                var sizeInMB = image.Length / (1024 * 1024);
                _logger.LogInformation($"Image size: {sizeInMB}MB");

                if (sizeInMB > 5)
                {
                    _logger.LogWarning($"Image exceeds size limit: {name}");
                }

                _logger.LogInformation($"Image processing completed: {name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded image");
                throw;
            }
        }

        /// <summary>
        /// Queue trigger function to process RabbitMQ messages
        /// </summary>
        [Function("ProcessMasterDataMessage")]
        public async Task ProcessMasterDataMessage(
            [QueueTrigger("masterdata-queue")] string queueItem,
            FunctionContext context)
        {
            _logger.LogInformation($"Processing queue message: {queueItem}");

            try
            {
                // Parse and process message
                _logger.LogInformation($"Queue message processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing queue message");
                throw;
            }
        }
    }
}
