namespace BuyMyHouse.AzureFunctions.DTO;

public class NotificationMessage
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string BlobUrl { get; set; } = string.Empty;
}