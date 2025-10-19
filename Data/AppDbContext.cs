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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<IncomeHistory> IncomeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MortgageApplication -> House (many-to-one)
            modelBuilder.Entity<MortgageApplication>()
                .HasOne(m => m.House)
                .WithMany()
                .HasForeignKey(m => m.HouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // IncomeHistory -> Customer relationship
            modelBuilder.Entity<IncomeHistory>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.IncomeHistories)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}