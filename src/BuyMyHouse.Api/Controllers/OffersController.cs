using BuyMyHouse.Api.DTOs;
using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OffersController : ControllerBase
{
    private readonly IRepository<MortgageOffer> _offerRepo;
    private readonly IRepository<MortgageApplication> _appRepo;

    public OffersController(IRepository<MortgageOffer> offerRepo, IRepository<MortgageApplication> appRepo)
    {
        _offerRepo = offerRepo;
        _appRepo = appRepo;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var offer = await _offerRepo.GetByIdAsync(id);
        if (offer == null) return NotFound();

        var dto = new OfferDto(offer.Id, offer.ApplicationId, offer.ApprovedAmount, offer.InterestRate, offer.ValidUntil, offer.OfferDocumentUrl);
        return Ok(dto);
    }

    [HttpGet("by-application/{appId:int}")]
    public async Task<IActionResult> GetByApplication(int appId)
    {
        var offers = await _offerRepo.FindAsync(o => o.ApplicationId == appId);
        var latest = offers.OrderByDescending(o => o.ValidUntil).FirstOrDefault();
        if (latest == null) return NotFound();

        return Ok(new OfferDto(latest.Id, latest.ApplicationId, latest.ApprovedAmount, latest.InterestRate, latest.ValidUntil, latest.OfferDocumentUrl));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
    {
        var app = await _appRepo.GetByIdAsync(dto.ApplicationId);
        if (app == null) return BadRequest("Application not found");

        var offer = new MortgageOffer
        {
            ApplicationId = dto.ApplicationId,
            ApprovedAmount = dto.ApprovedAmount,
            InterestRate = dto.InterestRate,
            ValidUntil = dto.ValidUntil,
            OfferDocumentUrl = dto.OfferDocumentUrl
        };

        await _offerRepo.AddAsync(offer);
        await _offerRepo.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = offer.Id }, new OfferDto(offer.Id, offer.ApplicationId, offer.ApprovedAmount, offer.InterestRate, offer.ValidUntil, offer.OfferDocumentUrl));
    }
}