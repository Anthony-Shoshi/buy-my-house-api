using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;

namespace BuyMyHouse.Domain.Services;

public class MortgageService
{
    private readonly IMortgageApplicationRepository _appRepo;

    public MortgageService(IMortgageApplicationRepository appRepo)
    {
        _appRepo = appRepo;
    }

    public async Task<IEnumerable<MortgageApplication>> GetPendingApplicationsAsync()
        => await _appRepo.GetPendingApplicationsAsync();

    public decimal CalculateEligibleAmount(decimal annualIncome)
    {
        // Simple rule: 5x annual income
        return annualIncome * 5;
    }

    public decimal CalculateInterestRate(decimal annualIncome)
    {
        // Higher income → lower rate
        return annualIncome switch
        {
            > 100000 => 2.9m,
            > 50000  => 3.5m,
            _        => 4.2m
        };
    }
}