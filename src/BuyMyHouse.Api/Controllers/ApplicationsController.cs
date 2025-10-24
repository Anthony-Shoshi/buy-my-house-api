using BuyMyHouse.Api.DTOs;
using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IRepository<MortgageApplication> _appRepo;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<House> _houseRepo;

    public ApplicationsController(IRepository<MortgageApplication> appRepo, IRepository<User> userRepo, IRepository<House> houseRepo)
    {
        _appRepo = appRepo;
        _userRepo = userRepo;
        _houseRepo = houseRepo;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] CreateApplicationDto dto)
    {
        var user = await _userRepo.GetByIdAsync(dto.UserId);
        if (user == null) return BadRequest("User not found");

        var house = await _houseRepo.GetByIdAsync(dto.HouseId);
        if (house == null) return BadRequest("House not found");

        var app = new MortgageApplication
        {
            UserId = dto.UserId,
            HouseId = dto.HouseId,
            AnnualIncome = dto.AnnualIncome,
            LoanAmountRequested = dto.LoanAmountRequested,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _appRepo.AddAsync(app);
        await _appRepo.SaveChangesAsync();

        var response = new ApplicationDto(app.Id, app.UserId, app.HouseId, app.AnnualIncome, app.LoanAmountRequested, app.Status, app.CreatedAt);
        return CreatedAtAction(nameof(Get), new { id = app.Id }, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var app = await _appRepo.GetByIdAsync(id);
        if (app == null) return NotFound();

        var dto = new ApplicationDto(app.Id, app.UserId, app.HouseId, app.AnnualIncome, app.LoanAmountRequested, app.Status, app.CreatedAt);
        return Ok(dto);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var list = await _appRepo.FindAsync(a => a.UserId == userId);
        var result = list
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new ApplicationDto(a.Id, a.UserId, a.HouseId, a.AnnualIncome, a.LoanAmountRequested, a.Status, a.CreatedAt))
            .ToList();

        return Ok(result);
    }
}