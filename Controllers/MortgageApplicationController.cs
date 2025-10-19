using BuyMyHouse.Api.Data;
using BuyMyHouse.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MortgageApplicationController : ControllerBase
    {
        private readonly IRepository<MortgageApplication> _mortgageRepository;

        public MortgageApplicationController(IRepository<MortgageApplication> mortgageRepository)
        {
            _mortgageRepository = mortgageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _mortgageRepository.GetAllAsync();
            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplication(MortgageApplication application)
        {
            await _mortgageRepository.AddAsync(application);
            await _mortgageRepository.SaveChangesAsync();
            return Ok(application);
        }
    }
}