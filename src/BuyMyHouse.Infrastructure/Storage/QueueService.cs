using Azure.Storage.Queues;
using BuyMyHouse.Domain.Repositories;

namespace BuyMyHouse.Infrastructure.Storage;

public class QueueService : IQueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(string connectionString)
    {
        _queueClient = new QueueClient(connectionString, "mortgage-notifications");
        _queueClient.CreateIfNotExists();
    }

    public async Task SendMessageAsync(string message)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(message);
        await _queueClient.SendMessageAsync(Convert.ToBase64String(bytes));
    }

    public async Task<string?> ReceiveMessageAsync()
    {
        var msg = await _queueClient.ReceiveMessageAsync();
        if (msg.Value != null)
            await _queueClient.DeleteMessageAsync(msg.Value.MessageId, msg.Value.PopReceipt);
        return msg.Value?.MessageText;
    }
}