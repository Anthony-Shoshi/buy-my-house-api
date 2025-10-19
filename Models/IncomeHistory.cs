namespace BuyMyHouse.Api.Models
{
    public class IncomeHistory
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Income { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        public Customer? Customer { get; set; }
    }
}