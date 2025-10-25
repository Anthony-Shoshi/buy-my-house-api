using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Enums;
using BuyMyHouse.Infrastructure.Database;

namespace BuyMyHouse.Infrastructure.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(BuyMyHouseDbContext context)
    {
        context.MortgageApplications.RemoveRange(context.MortgageApplications);
        context.Houses.RemoveRange(context.Houses);
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();

        var user1 = new User { FullName = "John Doe", Email = "gomesanthonyshoshi@gmail.com", Phone = "123-456-7890" };
        var user2 = new User { FullName = "Jane Smith", Email = "anthonyshoshigomes1996@gmail.com", Phone = "123-456-7890" };
        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync(); // 👈 important

        var house1 = new House
        {
            Title = "House A",
            Address = "Amsterdam",
            Price = 500000,
            Description = "Nice house",
            ImageUrl = "https://images.pexels.com/photos/106399/pexels-photo-106399.jpeg"
        };

        var house2 = new House
        {
            Title = "House B",
            Address = "Haarlem",
            Price = 350000,
            Description = "Cozy house",
            ImageUrl = "https://images.pexels.com/photos/206172/pexels-photo-206172.jpeg"
        };

        await context.Houses.AddRangeAsync(house1, house2);
        await context.SaveChangesAsync();

        var app1 = new MortgageApplication
        {
            UserId = user1.Id,
            HouseId = house1.Id,
            AnnualIncome = 60000,
            LoanAmountRequested = 300000,
            Status = ApplicationStatus.Pending
        };

        var app2 = new MortgageApplication
        {
            UserId = user2.Id,
            HouseId = house2.Id,
            AnnualIncome = 45000,
            LoanAmountRequested = 250000,
            Status = ApplicationStatus.Pending
        };

        await context.MortgageApplications.AddRangeAsync(app1, app2);
        await context.SaveChangesAsync();
    }
}