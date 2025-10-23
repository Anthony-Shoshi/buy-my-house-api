namespace BuyMyHouse.Domain.Entities;

public class MortgageApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HouseId { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal LoanAmountRequested { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public House? House { get; set; }
}