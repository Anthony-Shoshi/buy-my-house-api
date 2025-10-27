using BuyMyHouse.Api.Controllers;
using BuyMyHouse.Domain.Services;
using BuyMyHouse.Domain.Repositories;
using BuyMyHouse.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;

public class TestControllerTests
{
    [Fact]
    public async Task TestBlob_ShouldReturnOkResult()
    {
        var blobMock = new Mock<IBlobService>();
        blobMock.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("https://fakebloburl");

        var repoMock = new Mock<IMortgageApplicationRepository>();
        var mortgageService = new MortgageService(repoMock.Object);

        var controller = new TestController(null!, mortgageService, blobMock.Object, null!, null!);

        var result = await controller.TestBlob() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}
