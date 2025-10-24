namespace BuyMyHouse.Api.DTOs;

public record CreateApplicationDto(
    int UserId,
    int HouseId,
    decimal AnnualIncome,
    decimal LoanAmountRequested
);

public record ApplicationDto(
    int Id,
    int UserId,
    int HouseId,
    decimal AnnualIncome,
    decimal LoanAmountRequested,
    string Status,
    DateTime CreatedAt
);