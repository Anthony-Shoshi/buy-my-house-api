using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Services;
using BuyMyHouse.Infrastructure.Database;
using BuyMyHouse.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly BuyMyHouseDbContext _db;
    private readonly MortgageService _mortgageService;
    private readonly BlobService _blobService;
    private readonly QueueService _queueService;
    private readonly TableService _tableService;

    public TestController(
        BuyMyHouseDbContext db,
        MortgageService mortgageService,
        BlobService blobService,
        QueueService queueService,
        TableService tableService)
    {
        _db = db;
        _mortgageService = mortgageService;
        _blobService = blobService;
        _queueService = queueService;
        _tableService = tableService;
    }

    // EF Core test: add a house
    [HttpPost("house")]
    public async Task<IActionResult> AddHouse()
    {
        var house = new House
        {
            Title = "Test House",
            Address = "Amsterdam",
            Price = 500000,
            Description = "Sample description",
            ImageUrl = "Files/test.jpg"
        };
        _db.Houses.Add(house);
        await _db.SaveChangesAsync();
        return Ok(house);
    }

    // Blob test: upload dummy file
    [HttpGet("blob")]
    public async Task<IActionResult> TestBlob()
    {
        // Use relative path from project folder
        var url = await _blobService.UploadFileAsync("Files/test.pdf", "test.pdf");
        return Ok(new { url });
    }

    // Queue test: send/receive message
    [HttpGet("queue")]
    public async Task<IActionResult> TestQueue()
    {
        await _queueService.SendMessageAsync("Hello Queue");
        var msg = await _queueService.ReceiveMessageAsync();
        return Ok(new { msg });
    }

    // Table test: add income record
    [HttpGet("table")]
    public async Task<IActionResult> TestTable()
    {
        await _tableService.AddIncomeRecordAsync("user1", 75000);
        var records = await _tableService.GetIncomeRecordsAsync("user1");
        return Ok(records);
    }

    // Domain service test
    [HttpGet("mortgage")]
    public IActionResult TestMortgage()
    {
        var eligible = _mortgageService.CalculateEligibleAmount(60000);
        var rate = _mortgageService.CalculateInterestRate(60000);
        return Ok(new { eligible, rate });
    }
}