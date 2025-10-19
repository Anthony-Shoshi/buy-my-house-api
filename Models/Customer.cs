namespace BuyMyHouse.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public List<MortgageApplication> MortgageApplications { get; set; } = new();
                
        public List<IncomeHistory> IncomeHistories { get; set; } = new();

    }
}