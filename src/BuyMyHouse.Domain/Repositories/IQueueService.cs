namespace BuyMyHouse.Domain.Repositories;
public interface IQueueService
{
    Task SendMessageAsync(string message);
    Task<string?> ReceiveMessageAsync();
}