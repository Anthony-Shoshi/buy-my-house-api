namespace BuyMyHouse.Domain.Entities;

public class MortgageOffer
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public decimal ApprovedAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime ValidUntil { get; set; }
    public string OfferDocumentUrl { get; set; } = default!; // Blob file link

    public MortgageApplication? Application { get; set; }
}