using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.AzureFunctions;

public class NotificationFunction
{
    private readonly ILogger<NotificationFunction> _logger;

    public NotificationFunction(ILogger<NotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(NotificationFunction))]
    public async Task RunAsync([QueueTrigger("mortgage-notifications", Connection = "UseDevelopmentStorage=true")] QueueMessage message)
    {
        var msg = JsonSerializer.Deserialize<dynamic>(message.MessageText);

        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);

        await Task.Delay(1000);

        _logger.LogInformation($"Notification sent successfully for Application {msg?.Id}");
    }
}
