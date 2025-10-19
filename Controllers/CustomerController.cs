using BuyMyHouse.Api.Data;
using BuyMyHouse.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return Ok(customer);
        }
    }
}