using BuyMyHouse.Domain.Services;
using BuyMyHouse.Domain.Repositories;
using Moq;
using Xunit;

namespace BuyMyHouse.Tests.Services;

public class MortgageServiceTests
{
    [Fact]
    public void CalculateEligibleAmount_ShouldReturnCorrectValue()
    {
        var repoMock = new Mock<IMortgageApplicationRepository>();
        var service = new MortgageService(repoMock.Object);

        var result = service.CalculateEligibleAmount(60000);

        Assert.Equal(300000, result);
    }

    [Fact]
    public void CalculateInterestRate_ShouldReturnLowerRateForHigherIncome()
    {
        var repoMock = new Mock<IMortgageApplicationRepository>();
        var service = new MortgageService(repoMock.Object);

        var lowIncomeRate = service.CalculateInterestRate(30000);
        var highIncomeRate = service.CalculateInterestRate(100000);

        Assert.True(highIncomeRate < lowIncomeRate);
    }
}
