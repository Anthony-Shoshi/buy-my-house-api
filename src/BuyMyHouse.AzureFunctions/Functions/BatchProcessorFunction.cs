using System.Text.Json;
using BuyMyHouse.Domain.Entities;
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

    public BatchProcessorFunction(
        BuyMyHouseDbContext db,
        BlobService blobService,
        QueueService queueService,
        TableService tableService,
        ILogger<BatchProcessorFunction> logger)
    {
        _db = db;
        _blobService = blobService;
        _queueService = queueService;
        _tableService = tableService;
        _logger = logger;
    }

    // Runs every day at 23:00 (set to */1 * * * * * for 1 min interval while testing)
    [Function("BatchProcessorFunction")]
    public async Task RunAsync([TimerTrigger("0 0 23 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("BatchProcessorFunction started at {time}", DateTime.Now);

        var pendingApps = await _db.MortgageApplications
            .Where(a => a.Status == "Pending")
            .ToListAsync();

        foreach (var app in pendingApps)
        {
            try
            {
                var customerName = app.User?.FullName ?? "Unknown";
                var income = app.AnnualIncome;
                // Generate a simple mortgage offer JSON
                var offer = new
                {
                    ApplicationId = app.Id,
                    CustomerName = customerName,
                    OfferAmount = income * 5,
                    InterestRate = 3.5,
                    ValidUntil = DateTime.Now.AddDays(7)
                };

                string fileName = $"offer_{app.Id}.json";
                string blobUrl = await _blobService.UploadFileAsync(JsonSerializer.Serialize(offer), fileName);

                // Save income to Table storage
                await _tableService.AddIncomeRecordAsync(customerName, income);

                // Add queue message for notification
                await _queueService.SendMessageAsync(JsonSerializer.Serialize(new
                {
                    app.Id,
                    CustomerName = customerName,
                    BlobUrl = blobUrl
                }));

                app.Status = "Processed";
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