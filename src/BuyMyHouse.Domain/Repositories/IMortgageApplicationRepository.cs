using BuyMyHouse.Domain.Entities;

namespace BuyMyHouse.Domain.Repositories;

public interface IMortgageApplicationRepository : IRepository<MortgageApplication>
{
    Task<IEnumerable<MortgageApplication>> GetPendingApplicationsAsync();

    Task<IEnumerable<MortgageApplication>> GetProcessedApplicationsAsync();
}