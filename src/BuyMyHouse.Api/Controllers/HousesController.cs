using BuyMyHouse.Api.DTOs;
using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{
    private readonly IRepository<House> _houseRepo;

    public HousesController(IRepository<House> houseRepo)
    {
        _houseRepo = houseRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string? q)
    {
        var houses = await _houseRepo.GetAllAsync();

        var filtered = houses
            .Where(h => (!minPrice.HasValue || h.Price >= minPrice)
                     && (!maxPrice.HasValue || h.Price <= maxPrice)
                     && (string.IsNullOrWhiteSpace(q) || h.Title.Contains(q, StringComparison.OrdinalIgnoreCase) || h.Address.Contains(q, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(h => h.Price)
            .Select(h => new HouseDto(h.Id, h.Title, h.Address, h.Price, h.Description, h.ImageUrl))
            .ToList();

        return Ok(filtered);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var h = await _houseRepo.GetByIdAsync(id);
        if (h == null) return NotFound();
        return Ok(new HouseDto(h.Id, h.Title, h.Address, h.Price, h.Description, h.ImageUrl));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHouseDto dto)
    {
        var house = new House
        {
            Title = dto.Title,
            Address = dto.Address,
            Price = dto.Price,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl
        };

        await _houseRepo.AddAsync(house);
        await _houseRepo.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = house.Id }, new HouseDto(house.Id, house.Title, house.Address, house.Price, house.Description, house.ImageUrl));
    }
}