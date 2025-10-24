using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using BuyMyHouse.AzureFunctions.DTO;
using BuyMyHouse.Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.AzureFunctions;

public class NotificationFunction
{
    private readonly ILogger<NotificationFunction> _logger;
    private readonly EmailService _emailService;

    public NotificationFunction(ILogger<NotificationFunction> logger, EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    [Function(nameof(NotificationFunction))]
    public async Task RunAsync([QueueTrigger("mortgage-notifications", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        _logger.LogInformation("NotificationFunction triggered at {time}", DateTime.Now);

        var notification = JsonSerializer.Deserialize<NotificationMessage>(message.MessageText);

        if (notification == null)
        {
            _logger.LogWarning("Invalid message received");
            return;
        }

        string subject = $"Your Mortgage Offer from BuyMyHouse";
        string body = $"""
        Hello {notification.CustomerName},

        Your mortgage offer is ready to view. You can access it securely at:
        {notification.BlobUrl}

        Please note that this link will expire soon.

        Best regards,
        BuyMyHouse Team
        """;

        try
        {
            await _emailService.SendEmailAsync(notification.CustomerEmail, subject, body);
            _logger.LogInformation("Email sent successfully to {email}", notification.CustomerEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {email}", notification.CustomerEmail);
            throw; // so Azure Functions can retry
        }
    }
}
