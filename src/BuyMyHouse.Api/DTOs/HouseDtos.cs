namespace BuyMyHouse.Api.DTOs;

public record CreateHouseDto(
    string Title,
    string Address,
    decimal Price,
    string Description,
    string ImageUrl
);

public record HouseDto(
    int Id,
    string Title,
    string Address,
    decimal Price,
    string Description,
    string ImageUrl
);