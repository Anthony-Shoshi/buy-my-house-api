using BuyMyHouse.Api.Data;
using BuyMyHouse.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly IRepository<House> _houseRepository;

        public HouseController(IRepository<House> houseRepository)
        {
            _houseRepository = houseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHouses()
        {
            var houses = await _houseRepository.GetAllAsync();
            return Ok(houses);
        }

        [HttpPost]
        public async Task<IActionResult> AddHouse(House house)
        {
            await _houseRepository.AddAsync(house);
            await _houseRepository.SaveChangesAsync();
            return Ok(house);
        }
    }
}