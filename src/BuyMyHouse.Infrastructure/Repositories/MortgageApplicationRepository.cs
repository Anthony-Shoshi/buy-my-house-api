using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;
using BuyMyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.Infrastructure.Repositories;

public class MortgageApplicationRepository : Repository<MortgageApplication>, IMortgageApplicationRepository
{
    public MortgageApplicationRepository(BuyMyHouseDbContext context) : base(context) { }

    public async Task<IEnumerable<MortgageApplication>> GetPendingApplicationsAsync()
    {
        return await _dbSet.Where(a => a.Status == "Pending").ToListAsync();
    }
}