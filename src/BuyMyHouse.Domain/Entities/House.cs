namespace BuyMyHouse.Domain.Entities;

public class House
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Address { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
}