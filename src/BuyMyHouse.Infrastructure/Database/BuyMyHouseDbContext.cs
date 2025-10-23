using BuyMyHouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.Infrastructure.Database;

public class BuyMyHouseDbContext : DbContext
{
    public BuyMyHouseDbContext(DbContextOptions<BuyMyHouseDbContext> options)
        : base(options)
    {
    }

    public DbSet<House> Houses => Set<House>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MortgageApplication> MortgageApplications => Set<MortgageApplication>();
    public DbSet<IncomeRecord> IncomeRecords => Set<IncomeRecord>();
    public DbSet<MortgageOffer> MortgageOffers => Set<MortgageOffer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MortgageApplication>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<MortgageApplication>()
            .HasOne(m => m.House)
            .WithMany()
            .HasForeignKey(m => m.HouseId);

        modelBuilder.Entity<MortgageOffer>()
            .HasOne(o => o.Application)
            .WithMany()
            .HasForeignKey(o => o.ApplicationId);
    }
}