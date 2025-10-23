namespace BuyMyHouse.Domain.Entities;

public class IncomeRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal AnnualIncome { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}