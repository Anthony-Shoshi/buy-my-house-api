namespace BuyMyHouse.AzureFunctions.DTOs;

public class MortgageOfferDto
{
    public int ApplicationId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal OfferAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime ValidUntil { get; set; }
}
