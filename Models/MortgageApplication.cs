namespace BuyMyHouse.Api.Models
{
    public class MortgageApplication
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal LoanAmount { get; set; }
        public bool IsApproved { get; set; }
    }
}