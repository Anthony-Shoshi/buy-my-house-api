using BuyMyHouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<House> Houses { get; set; }
        public DbSet<MortgageApplication> MortgageApplications { get; set; }
    }
}