using Azure.Storage.Queues;

namespace BuyMyHouse.Infrastructure.Storage;

public class QueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(string connectionString)
    {
        _queueClient = new QueueClient(connectionString, "mortgage-notifications");
        _queueClient.CreateIfNotExists();
    }

    public async Task SendMessageAsync(string message)
    {
        await _queueClient.SendMessageAsync(message);
    }

    public async Task<string?> ReceiveMessageAsync()
    {
        var msg = await _queueClient.ReceiveMessageAsync();
        if (msg.Value != null)
            await _queueClient.DeleteMessageAsync(msg.Value.MessageId, msg.Value.PopReceipt);
        return msg.Value?.MessageText;
    }
}