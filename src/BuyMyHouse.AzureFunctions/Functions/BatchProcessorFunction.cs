using System.Text.Json;
using BuyMyHouse.AzureFunctions.DTOs;
using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Enums;
using BuyMyHouse.Domain.Services;
using BuyMyHouse.Infrastructure.Database;
using BuyMyHouse.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.AzureFunctions.Functions;

public class BatchProcessorFunction
{
    private readonly BuyMyHouseDbContext _db;
    private readonly BlobService _blobService;
    private readonly QueueService _queueService;
    private readonly TableService _tableService;
    private readonly ILogger<BatchProcessorFunction> _logger;
    private readonly MortgageService _mortgageService;

    public BatchProcessorFunction(
        BuyMyHouseDbContext db,
        BlobService blobService,
        QueueService queueService,
        TableService tableService,
        ILogger<BatchProcessorFunction> logger,
        MortgageService mortgageService)
    {
        _db = db;
        _blobService = blobService;
        _queueService = queueService;
        _tableService = tableService;
        _logger = logger;
        _mortgageService = mortgageService;
    }

    // Runs every day at 23:00 (set to */1 * * * * * for 1 min interval while testing)
    [Function("BatchProcessorFunction")]
    public async Task RunAsync([TimerTrigger("*/1 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("BatchProcessorFunction started at {time}", DateTime.Now);

        var pendingApps = await _mortgageService.GetPendingApplicationsAsync();

        foreach (var app in pendingApps)
        {
            try
            {
                var customerName = app.User?.FullName ?? "Unknown";
                var income = app.AnnualIncome;
                // Generate a simple mortgage offer JSON
                var offer = new MortgageOfferDto
                {
                    ApplicationId = app.Id,
                    CustomerName = customerName,
                    OfferAmount = _mortgageService.CalculateEligibleAmount(income),
                    InterestRate = _mortgageService.CalculateInterestRate(income),
                    ValidUntil = DateTime.Now.AddDays(7)
                };

                var offerText = $"""
                Mortgage Offer
                ---------------
                ApplicationId: {app.Id}
                CustomerName: {customerName}
                OfferAmount: {_mortgageService.CalculateEligibleAmount(income)}
                InterestRate: {_mortgageService.CalculateInterestRate(income)}
                ValidUntil: {DateTime.Now.AddDays(7):yyyy-MM-dd}
                """;

                string fileName = $"offer_{app.Id}.txt";
                string blobUrl = await _blobService.UploadFileAsync(offerText, fileName);

                // Save income to Table storage
                await _tableService.AddIncomeRecordAsync(customerName, income);

                // Add queue message for notification
                _logger.LogInformation("Sending notification to queue for application {id} at {time}", app.Id, DateTime.Now);
                var notification = new NotificationMessage
                {
                    Id = app.Id,
                    CustomerName = customerName,
                    CustomerEmail = app.User?.Email ?? "unknown@example.com",
                    BlobUrl = blobUrl
                };

                var payload = JsonSerializer.Serialize(notification);
                _logger.LogInformation("Queue payload: {payload}", payload);

                await _queueService.SendMessageAsync(payload);
                _logger.LogInformation("Notification queued completed for application {id} at {time}", app.Id, DateTime.Now);

                app.Status = ApplicationStatus.Processed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing application {id}", app.Id);
            }
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("BatchProcessorFunction completed at {time}", DateTime.Now);
    }
}