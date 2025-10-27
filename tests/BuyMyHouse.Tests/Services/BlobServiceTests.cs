using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BuyMyHouse.Infrastructure.Storage;
using Azure;
using Moq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BuyMyHouse.Tests.Services;

public class BlobServiceTests
{
    [Fact]
    public async Task UploadFileAsync_ShouldReturnUrl_WhenUploadSucceeds()
    {
        // Arrange

        // Mock BlobClient
        var blobClientMock = new Mock<BlobClient>();
        blobClientMock
            .Setup(b => b.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<Response<BlobContentInfo>>());

        blobClientMock
            .Setup(b => b.Uri)
            .Returns(new System.Uri("https://fakeblob.blob.core.windows.net/container/test.txt"));

        // Mock BlobContainerClient
        var containerMock = new Mock<BlobContainerClient>();
        containerMock.Setup(c => c.GetBlobClient(It.IsAny<string>()))
                     .Returns(blobClientMock.Object);
        containerMock.Setup(c => c.CreateIfNotExistsAsync(It.IsAny<PublicAccessType>(), null, null, default))
                     .Returns(Task.FromResult(Mock.Of<Response<BlobContainerInfo>>()));

        // Mock BlobServiceClient
        var serviceClientMock = new Mock<BlobServiceClient>();
        serviceClientMock.Setup(c => c.GetBlobContainerClient(It.IsAny<string>()))
                         .Returns(containerMock.Object);

        // Inject mocked service client into BlobService
        var service = new BlobService(serviceClientMock.Object);

        // Act
        var result = await service.UploadFileAsync("Test content", "test.txt");

        // Assert
        Assert.Contains("https://", result);
    }

    [Fact]
    public async Task UploadFileWithSasAsync_ShouldReturnSasUrl_WhenUploadSucceeds()
    {
        // Arrange

        var blobClientMock = new Mock<BlobClient>();
        blobClientMock
            .Setup(b => b.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<Response<BlobContentInfo>>());

        blobClientMock
            .Setup(b => b.GenerateSasUri(It.IsAny<Azure.Storage.Sas.BlobSasBuilder>()))
            .Returns(new System.Uri("https://fakeblob.blob.core.windows.net/container/test.txt?sasToken=fake"));

        var containerMock = new Mock<BlobContainerClient>();
        containerMock.Setup(c => c.GetBlobClient(It.IsAny<string>()))
                     .Returns(blobClientMock.Object);
        containerMock.Setup(c => c.CreateIfNotExistsAsync(It.IsAny<PublicAccessType>(), null, null, default))
                     .Returns(Task.FromResult(Mock.Of<Response<BlobContainerInfo>>()));

        var serviceClientMock = new Mock<BlobServiceClient>();
        serviceClientMock.Setup(c => c.GetBlobContainerClient(It.IsAny<string>()))
                         .Returns(containerMock.Object);

        var service = new BlobService(serviceClientMock.Object);

        // Act
        var result = await service.UploadFileWithSasAsync("Test content", "test.txt", TimeSpan.FromMinutes(5));

        // Assert
        Assert.Contains("sasToken=fake", result);
    }
}
