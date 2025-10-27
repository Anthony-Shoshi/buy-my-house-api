using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using BuyMyHouse.AzureFunctions.DTOs;
using BuyMyHouse.Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.AzureFunctions.Functions;

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
    public async Task RunAsync([QueueTrigger("mortgage-notifications", Connection = "AzureWebJobsStorage")] string messageText)
    {
        NotificationMessage? notification = null;

        try
        {
            notification = JsonSerializer.Deserialize<NotificationMessage>(messageText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize message: {messageText}", messageText);
            return;
        }

        if (notification == null)
        {
            _logger.LogWarning("Message deserialized to null: {messageText}", messageText);
            return;
        }

        try
        {
            string subject = $"Your Mortgage Offer from BuyMyHouse";
            string body = $"""
            Hello {notification.CustomerName},

            Your mortgage offer is ready to view:
            {notification.BlobUrl}

            This link will expire in 1 hour.

            Best regards,
            BuyMyHouse Team
            """;

            await _emailService.SendEmailAsync(notification.CustomerEmail, subject, body);

            _logger.LogInformation("Email sent to {customerEmail} for application {appId}", notification.CustomerEmail, notification.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email for message: {messageText}", messageText);
            throw; // will retry / go to poison queue if transient
        }
    }
}