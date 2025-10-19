namespace BuyMyHouse.Api.Models
{
    public class MortgageApplication
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal LoanAmount { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public House? House { get; set; }
    }
}