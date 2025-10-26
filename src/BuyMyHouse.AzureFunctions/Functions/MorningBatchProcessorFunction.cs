using System;
using System.Text.Json;
using BuyMyHouse.AzureFunctions.DTOs;
using BuyMyHouse.Domain.Services;
using BuyMyHouse.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.AzureFunctions.Functions;

public class MorningBatchProcessorFunction
{
    private readonly ILogger _logger;
    private readonly MortgageService _mortgageService;
    private readonly BlobService _blobService;
    private readonly QueueService _queueService;

    public MorningBatchProcessorFunction(ILoggerFactory loggerFactory, MortgageService mortgageService, BlobService blobService, QueueService queueService)
    {
        _logger = loggerFactory.CreateLogger<MorningBatchProcessorFunction>();
        _mortgageService = mortgageService;
        _blobService = blobService;
        _queueService = queueService;
    }

    [Function("MorningBatchProcessorFunction")]
    public async Task RunAsync([TimerTrigger("*/2 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("Morning notification batch started at {time}", DateTime.Now);

        var processedApps = await _mortgageService.GetProcessedApplicationsAsync();
        
        foreach (var app in processedApps)
        {
            var customerName = app.User?.FullName ?? "Unknown";

            var notification = new NotificationMessage
            {
                Id = app.Id,
                CustomerName = customerName,
                CustomerEmail = app.User?.Email ?? "unknown@example.com",
                BlobUrl = await _blobService.GetSasUrlAsync($"offer_{app.Id}.txt", TimeSpan.FromHours(5))
            };

            var payload = JsonSerializer.Serialize(notification);
            await _queueService.SendMessageAsync(payload);

            _logger.LogInformation("Notification queued for application {appId} of customer {customerName}", app.Id, customerName);
        }
    }
}