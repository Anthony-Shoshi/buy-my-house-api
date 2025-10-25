using BuyMyHouse.Domain.Enums;

namespace BuyMyHouse.Api.DTOs;

public class CreateApplicationDto
{
    // User info
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;

    // Application info
    public int HouseId { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal LoanAmountRequested { get; set; }
}

public record ApplicationDto(
    int Id,
    int UserId,
    int HouseId,
    decimal AnnualIncome,
    decimal LoanAmountRequested,
    ApplicationStatus Status,
    DateTime CreatedAt
);