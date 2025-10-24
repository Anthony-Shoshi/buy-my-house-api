namespace BuyMyHouse.Api.DTOs;

public record OfferDto(
    int Id,
    int ApplicationId,
    decimal ApprovedAmount,
    decimal InterestRate,
    DateTime ValidUntil,
    string OfferDocumentUrl
);

public record CreateOfferDto(
    int ApplicationId,
    decimal ApprovedAmount,
    decimal InterestRate,
    DateTime ValidUntil,
    string OfferDocumentUrl
);
